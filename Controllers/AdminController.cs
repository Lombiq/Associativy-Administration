using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Associativy.Administration.EventHandlers;
using Associativy.Administration.Services;
using Associativy.Controllers;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace Associativy.Administration.Controllers
{
    [Admin, OrchardFeature("Associativy.Administration")]
    public class AdminController : AdminControllerBase
    {
        private readonly IImportExportService _importExportService;

        public Localizer T { get; set; }

        public AdminController(
            IOrchardServices orchardServices,
            IAdminEventHandler eventHandler,
            IImportExportService importExportService)
            : base(orchardServices, eventHandler)
        {
            _importExportService = importExportService;

            T = NullLocalizer.Instance;
        }

        public ActionResult Index()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're You're not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            var page = NewPage("Index");
            _eventHandler.OnPageBuilt(page);

            return PageResult(page);
        }

        public ActionResult ManageGraph()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            var page = NewPage("ManageGraph");
            _eventHandler.OnPageBuilt(page);

            return PageResult(page);
        }

        public ActionResult ExportConnections(string graphName)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            try
            {
                return File(
                    Encoding.UTF8.GetBytes(_importExportService.ExportAllConnections(MakeContext(graphName))),
                    "application/json",
                    graphName + "-" + DateTime.UtcNow.ToString() + ".json");
            }
            catch (Exception ex)
            {
                _orchardServices.Notifier.Error(T("Export failed: {0}", ex.Message));
                return RedirectToAction("ManageGraph", new { GraphName = graphName });
            }
        }

        [HttpPost]
        public ActionResult ImportConnections(string graphName)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            if (String.IsNullOrEmpty(Request.Files["ConnectionsFile"].FileName))
            {
                _orchardServices.Notifier.Error(T("Please choose a connections file to import."));
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _importExportService.ImportConnections(MakeContext(graphName), new StreamReader(Request.Files["ConnectionsFile"].InputStream).ReadToEnd());

                    _orchardServices.Notifier.Information(T("The connections were successfully imported."));
                }
                catch (Exception ex)
                {
                    _orchardServices.Notifier.Error(T("Import failed: {0}", ex.Message));
                    return RedirectToAction("ManageGraph", new { GraphName = graphName });
                }
            }

            return RedirectToAction("ManageGraph", new { GraphName = graphName });
        }

        private GraphContext MakeContext(string graphName)
        {
            return new GraphContext { GraphName = graphName };
        }
    }
}