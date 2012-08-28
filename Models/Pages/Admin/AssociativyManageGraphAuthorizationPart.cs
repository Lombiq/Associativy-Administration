using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class AssociativyManageGraphAuthorizationPart : ContentPart
    {
        public IList<RoleEntry> Roles { get; set; }

        public AssociativyManageGraphAuthorizationPart()
        {
            Roles = new List<RoleEntry>();
        }
    }

    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class RoleEntry
    {
        public string Name { get; set; }
        public bool IsAuthorized { get; set; }
    }
}