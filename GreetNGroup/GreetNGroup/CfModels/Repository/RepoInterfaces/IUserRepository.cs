using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetNGroup.CfModels.Repository.RepoInterfaces
{
    interface IUserRepository : IRepository<User>
    {
        User FindUserByUserId(string id);
    }
}
