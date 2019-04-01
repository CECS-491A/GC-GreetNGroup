using System;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;

namespace ManagerLayer.ClaimManager
{
    public class ClaimManager
    {
        /// <summary>
        /// This region handles inserting claim information into the database
        /// </summary>
        #region Insert Claim Information

        public void InsertClaim(int claimId, string claimName)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var claim = new Claim(claimId, claimName);

                    ctx.Claims.Add(claim);

                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        #endregion

        /// <summary>
        /// This region checks user information in the database
        /// </summary>
        #region Claim Information Check

        public bool IsClaimInTable(int claimId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    return ctx.Claims.Any(u => u.ClaimId.Equals(claimId));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        #endregion

        #region Delete Claim Information

        public void DeleteClaimById(int claimId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var claim = ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(claimId));

                    if (claim != null) ctx.Claims.Remove(claim);

                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        #endregion
    }
}
