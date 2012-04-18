using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;
using Orchard.Environment.Extensions.Models;

namespace Associativy.Administration
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageAssociativyGraphs = new Permission { Description = "Manage Associativy graphs", Name = "ManageAssociativyGraphs" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] {
                ManageAssociativyGraphs
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageAssociativyGraphs }
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] { ManageAssociativyGraphs }
                }
            };
        }

    }
}