using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetNGroup.CfModels.Repository.RepoInterfaces
{
    interface IUserClaimRepository : IRepository<UserClaim>
    {
        List<string> GetUserClaimsById(string id);
    }
}
