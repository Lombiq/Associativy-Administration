using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using System.Web.Mvc;
using Orchard.Mvc;
using Orchard.DisplayManagement;
using Associativy.GraphDiscovery;
using Associativy.Administration.ViewModels;
using Orchard.Security;
using Orchard.Localization;
using Piedone.HelpfulLibraries.Serialization;
using Orchard.UI.Admin;
using Associativy.Services;
using System.IO;
using System.Text;
using Orchard;
using Orchard.UI.Notify;
using Associativy.Administration.Services;
using Associativy.Controllers;
using Associativy.Models.Mind;

namespace Associativy.Administration.Controllers
{
    [OrchardFeature("Associativy.Administration")]
    [Admin]
    public class AdminController : AssociativyControllerBase
    {
        private readonly dynamic _shapeFactory;
        private readonly IOrchardServices _orchardServices;
        private readonly IImportExportService _importExportService;

        public Localizer T { get; set; }

        public AdminController(
            IAssociativyServices associativyServices,
            IShapeFactory shapeFactory,
            IOrchardServices orchardServices,
            IImportExportService importExportService)
            : base(associativyServices)
        {
            _shapeFactory = shapeFactory;
            _orchardServices = orchardServices;
            _importExportService = importExportService;

            T = NullLocalizer.Instance;
        }

        public ActionResult Index()
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            return new ShapeResult(
                this,
                _shapeFactory.DisplayTemplate(
                    TemplateName: "Admin/Index",
                    Model: null,
                    Prefix: null));
        }

        public ActionResult ManageGraph(string graphName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            var graphContext = MakeContext(graphName);
            var graphContent = _mind.GetAllAssociations(graphContext, new MindSettings { ZoomLevel = int.MaxValue } );

            return new ShapeResult(
                this,
                _shapeFactory.DisplayTemplate(
                    TemplateName: "Admin/ManageGraph",
                    Model: new GraphManagementViewModel
                    {
                        Graph = _graphManager.FindGraph(graphContext),
                        NodeCount = graphContent.VertexCount,
                        ConnectionCount = graphContent.EdgeCount
                    },
                    Prefix: null));
        }

        public ActionResult ExportConnections(string graphName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage Associativy settings.")))
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
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage Associativy settings.")))
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