using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Associativy.Administration.Models;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Roles.Models;
using Orchard.Security;

namespace Associativy.Administration.Services
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class FrontendAuthorizer : IFrontendAuthorizer
    {
        private readonly IRepository<FrontendAuthorizationRecord> _repository;


        public FrontendAuthorizer(IRepository<FrontendAuthorizationRecord> repository)
        {
            _repository = repository;
        
        }


        public void SetAuthorizedToView(IGraphContext graphContext, IEnumerable<string> roles)
        {
            var record = GetByGraphContext(graphContext);
            if (record == null)
            {
                record = new FrontendAuthorizationRecord { GraphName = graphContext.Name };
                _repository.Create(record);
            }
            record.RolesDefinition = new JavaScriptSerializer().Serialize(roles);
        }

        public IEnumerable<string> GetAuthorizedToView(IGraphContext graphContext)
        {
            var record = GetByGraphContext(graphContext);
            if (record == null) return Enumerable.Empty<string>();
            return GetRoles(record);
        }

        public bool IsAuthorizedToView(IUser user, IGraphContext graphContext)
        {
            var record = GetByGraphContext(graphContext);
            if (record == null) return false;
            if (user == null) return GetRoles(record).Contains("Anonymous");
            return user.As<IUserRoles>().Roles.Intersect(GetRoles(record)).Count() != 0;
        }

        private FrontendAuthorizationRecord GetByGraphContext(IGraphContext graphContext)
        {
            return _repository.Fetch(r => r.GraphName == graphContext.Name).FirstOrDefault();
        }

        private IEnumerable<string> GetRoles(FrontendAuthorizationRecord record)
        {
            return new JavaScriptSerializer().Deserialize<IEnumerable<string>>(record.RolesDefinition);
        }
    }
}