using Associativy.GraphDiscovery;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using System.Linq;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration")]
    public class AdminMenu : INavigationProvider
    {
        private readonly IGraphManager _graphManager;

        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public AdminMenu(IGraphManager graphManager)
        {
            _graphManager = graphManager;
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder/*.AddImageSet("associativy")*/.Add(T("Associativy"), "1.5", BuildMenu);

            // This is for tabs (LocalNav)
            //builder//.AddImageSet("addociativy")
            //    .Add(T("Associativy"), "5",
            //        menu => menu.Action("Index", "Admin", new { area = "Associativy.Administration" }).Permission(Permissions.ManageAssociativyGraphs)
            //            .Add(T("Associativy"), "1.0", item => item.Action("Index", "Admin", new { area = "Associativy.Administration" })
            //                .LocalNav().Permission(Permissions.ManageAssociativyGraphs)));
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Action("Index", "Admin", new { area = "Associativy.Administration" }).Permission(Permissions.ManageAssociativyGraphs);

            menu.LinkToFirstChild(false);

            var graphs = _graphManager.FindDistinctGraphs(new GraphContext()).OrderBy(descriptor => descriptor.DisplayGraphName.Text);

            int i = 0;
            foreach (var graph in graphs)
            {
                menu.Add(graph.DisplayGraphName, i.ToString(),
                     item => item.Action("ManageGraph", "Admin", new { area = "Associativy.Administration", GraphName = graph.GraphName }).Permission(Permissions.ManageAssociativyGraphs));
                i++;
            }
        }
    }
}