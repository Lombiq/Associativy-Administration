﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Associativy.Administration.Models;
using Associativy.Administration.Models.Admin;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration")]
    public class DefaultAdminEventHandler : IAdminEventHandler
    {
        public void OnPageInitializing(IContent page)
        {
            if (IsPage(page, "ManageGraph"))
            {
                page.ContentItem.Weld(new AssociatvyManageGraphPart()); 
            }
            else if (IsPage(page, "Index"))
            {
                page.ContentItem.Weld(new AssociativyIndexPart()); 
            }
        }

        public void OnPageInitialized(IContent page)
        {
        }

        public void OnPageBuilt(IContent page)
        {
        }

        private bool IsPage(IContent page, string pageName)
        {
            return page.ContentItem.ContentType.EndsWith(pageName);
        }
    }
}