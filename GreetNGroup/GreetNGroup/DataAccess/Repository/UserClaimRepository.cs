using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GreetNGroup.DataAccess.Repository.RepoInterfaces;

namespace GreetNGroup.DataAccess.Repository
{
    public class UserClaimRepository : Repository<UserClaim>, IUserClaimRepository
    {
        // Convenience in insuring all context is GreetNGroupContext
        public GreetNGroupContext GreetNGroupContext => Context as GreetNGroupContext;

        public UserClaimRepository(DbContext context) : base(context)
        {
        }

        public List<string> GetUserClaimsById(string id)
        {
            List<UserClaim> userClaims = GreetNGroupContext.UserClaims.Where(c => c.UId.Equals(id)).ToList();
            var claimsTable = new List<Claim>();
            var claims = new List<string>();

            // Store claims through claim-user relationships
            foreach (var t in userClaims)
            {
                var curClaim = t.ClaimId;
                claimsTable.Add(GreetNGroupContext.Claims.Where(u => curClaim.Equals(u.ClaimId)).ToList()[0]);
            }

            // Store claim names from claims list
            foreach (var t1 in claimsTable)
            {
                claims.Add(t1.ClaimName);
            }
            return claims;
        }
    }
}