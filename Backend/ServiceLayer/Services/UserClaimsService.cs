using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gucci.ServiceLayer.Services
{
    public class UserClaimsService
    {
        private ILoggerService _gngLoggerService = new LoggerService();

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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfulAdd;
            }
        }

        public bool AddDefaultClaims(User newUser)
        {
            var isSuccessfulAdd = false;
            try
            {
                using(var ctx = new GreetNGroupContext())
                {
                    var userID = newUser.UserId;
                    var claimToAdd = ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(1));
                    var claimToAdd2 = ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(2));
                    var claimToAdd3 = ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(8));
                    var claimToAdd4 = ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(9));

                    var userClaim = new UserClaim(userID, claimToAdd);
                    var userClaim2 = new UserClaim(userID, claimToAdd2);
                    var userClaim3 = new UserClaim(userID, claimToAdd3);
                    var userClaim4 = new UserClaim(userID, claimToAdd4);

                    ctx.UserClaims.Add(userClaim);
                    ctx.UserClaims.Add(userClaim2);
                    ctx.UserClaims.Add(userClaim3);
                    ctx.UserClaims.Add(userClaim4);
                    ctx.SaveChanges();
                    isSuccessfulAdd = true;
                    return isSuccessfulAdd;
                }
            }
            catch (Exception od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
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
            catch (Exception od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
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
            catch (Exception od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }
    }
}
