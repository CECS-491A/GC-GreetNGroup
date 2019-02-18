using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GreetNGroup.DataAccess.Repository.RepoInterfaces;

namespace GreetNGroup.DataAccess.Repository
{
    /// <summary>
    /// This is an application specific repository used to find data on Claims
    /// </summary>
    public class ClaimRepository : Repository<Claim>, IClaimRepository
    {
        // Convenience in insuring all context is GreetNGroupContext
        public GreetNGroupContext GreetNGroupContext => Context as GreetNGroupContext;

        public ClaimRepository(DbContext context) : base(context)
        {
        }

        public Claim FindClaimById(int id)
        {
            return GreetNGroupContext.Set<Claim>().FirstOrDefault(c => c.ClaimId.Equals(id));
        }
    }
}