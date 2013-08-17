using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Associativy.Administration.Services;
using Associativy.GraphDiscovery;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Exceptions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.Controllers
{
    [Admin]
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        private readonly IPageEventHandler _eventHandler;
        private readonly IImportExportService _importExportService;

        public Localizer T { get; set; }


        public AdminController(
            IOrchardServices orchardServices,
            IPageEventHandler eventHandler,
            IImportExportService importExportService)
        {
            _orchardServices = orchardServices;
            _contentManager = orchardServices.ContentManager;
            _eventHandler = eventHandler;
            _importExportService = importExportService;

            T = NullLocalizer.Instance;
        }


        public ActionResult Index()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();
            
            var page = NewPage("Index");
            _eventHandler.OnPageBuilt(new PageContext(page, AdministrationPageConfigs.Group));

            return PageResult(page);
        }

        public ActionResult ManageGraph()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            var page = NewPage("ManageGraph");
            _eventHandler.OnPageBuilt(new PageContext(page, AdministrationPageConfigs.Group));

            return PageResult(page);
        }

        [HttpPost, ActionName("ManageGraph")]
        [FormValueRequired("submit.Save")]
        public ActionResult ManageGraphPost()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're not allowed to manage Associativy settings.")))
                return new HttpUnauthorizedResult();

            var page = NewPage("ManageGraph");
            _eventHandler.OnPageBuilt(new PageContext(page, AdministrationPageConfigs.Group));
            _contentManager.UpdateEditor(page, this);

            return Refresh();
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
                if (ex.IsFatal()) throw;
                _orchardServices.Notifier.Error(T("Export failed: {0}", ex.Message));
                return RedirectToAction("ManageGraph", new { GraphName = graphName });
            }
        }

        [HttpPost, ActionName("ManageGraph")]
        [FormValueRequired("submit.ImportConnections")]
        public ActionResult ImportConnections(string graphName, HttpPostedFileBase connectionsFile)
        {
            if (connectionsFile != null)
            {
                if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs, T("You're not allowed to manage Associativy settings.")))
                    return new HttpUnauthorizedResult();

                if (String.IsNullOrEmpty(connectionsFile.FileName))
                {
                    _orchardServices.Notifier.Error(T("Please choose a connections file to import."));
                }
                else if (ModelState.IsValid)
                {
                    try
                    {
                        _importExportService.ImportConnections(MakeContext(graphName), new StreamReader(connectionsFile.InputStream).ReadToEnd());

                        _orchardServices.Notifier.Information(T("The connections were successfully imported."));
                    }
                    catch (Exception ex)
                    {
                        if (ex.IsFatal()) throw;
                        _orchardServices.Notifier.Error(T("Import failed: {0}", ex.Message));
                        return RedirectToAction("ManageGraph", new { GraphName = graphName });
                    }
                } 
            }

            return RedirectToAction("ManageGraph", new { GraphName = graphName });
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        private IContent NewPage(string pageName)
        {
            return _contentManager.NewPage(pageName, AdministrationPageConfigs.Group, _eventHandler);
        }

        private ShapeResult PageResult(IContent page)
        {
            return new ShapeResult(this, _contentManager.BuildPageDisplay(page));
        }

        private GraphContext MakeContext(string graphName)
        {
            return new GraphContext { Name = graphName };
        }

        private RedirectResult Refresh()
        {
            return Redirect(_orchardServices.WorkContext.HttpContext.Request.RawUrl);
        }
    }
}