using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UserClaimsService
    {
        public List<Claim> GetUsersClaims(string username)
        {
            List<Claim> claimsList = new List<Claim>();

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var usersClaims = ctx.UserClaims.Where(c => c.User.UserName.Equals(username));
                    foreach (UserClaim claim in usersClaims)
                    {
                        claimsList.Add(claim.Claim);
                    }

                    return claimsList;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return claimsList;
            }
        }

        public bool IsClaimOnUser(int uId, int claimId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userClaims = ctx.UserClaims.Where(u => u.UId.Equals(uId));
                    return userClaims.Any(c => c.ClaimId.Equals(claimId));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }
    }
}
