using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class FrontendAuthorizationRecord
    {
        public virtual int Id { get; set; }
        public virtual string GraphName { get; set; }
        public virtual string RolesDefinition { get; set; }
    }
}