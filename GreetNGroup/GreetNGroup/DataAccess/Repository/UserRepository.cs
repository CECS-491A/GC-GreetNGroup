using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GreetNGroup.DataAccess.Repository.RepoInterfaces;

namespace GreetNGroup.DataAccess.Repository
{
    /// <summary>
    /// This is a application specific repository used for specific data(User) of this context
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        // Convenience in insuring all context is GreetNGroupContext
        public GreetNGroupContext GreetNGroupContext => Context as GreetNGroupContext;

        public UserRepository(DbContext context) : base(context)
        {
        }

        // Finds users based on UserId
        public User FindUserByUserId(string id)
        {
            return GreetNGroupContext.Users.FirstOrDefault(c => c.UserId.Equals(id));
        }
    }
}