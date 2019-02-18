using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetNGroup.DataAccess.Repository.RepoInterfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User FindUserByUserId(string id);
    }
}
