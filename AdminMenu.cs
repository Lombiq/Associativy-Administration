﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Navigation;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Security;
using Associativy.GraphDiscovery;

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
            builder/*.AddImageSet("addociativy")*/.Add(T("Associativy"), "1.5", BuildMenu);

            // This is for tabs (LocalNav)
            //builder//.AddImageSet("addociativy")
            //    .Add(T("Associativy"), "5",
            //        menu => menu.Action("Index", "Admin", new { area = "Associativy.Administration" }).Permission(StandardPermissions.SiteOwner)
            //            .Add(T("Associativy"), "1.0", item => item.Action("Index", "Admin", new { area = "Associativy.Administration" })
            //                .LocalNav().Permission(StandardPermissions.SiteOwner)));
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Action("Index", "Admin", new { area = "Associativy.Administration" }).Permission(StandardPermissions.SiteOwner);

            var graphs = _graphManager.FindGraphs(new GraphContext());

            int i = 0;
            foreach (var graph in graphs)
            {
                menu.Add(graph.DisplayGraphName, i.ToString(),
                     item => item.Action("ManageGraph", "Admin", new { area = "Associativy.Administration", GraphName = graph.GraphName }).Permission(StandardPermissions.SiteOwner));
                i++;
            }
        }
    }
}