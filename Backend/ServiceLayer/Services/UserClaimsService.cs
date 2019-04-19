using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public class UserClaimsService
    {
        public bool AddClaimToUser(string username, Claim claim)
        {
            var isSuccessfulAdd = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.FirstOrDefault(u => u.UserName.Equals(username));
                    var userId = user.UserId;
                    var claimToAdd = ctx.Claims.FirstOrDefault(c => c.ClaimName.Equals(claim.ClaimName));
                    UserClaim userClaim = new UserClaim(userId, claimToAdd);
                    ctx.UserClaims.Add(userClaim);
                    ctx.SaveChanges();
                    isSuccessfulAdd = true;
                }
                return isSuccessfulAdd;
            }
            catch(ObjectDisposedException od)
            {
                return isSuccessfulAdd;
            }
        }

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
