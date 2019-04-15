using System;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;

namespace ServiceLayer.Services
{
    public class ClaimService
    {
        /*
         * The functions within this service make use of the database context
         * and similarly attempt to catch
         *      ObjectDisposedException
         * to ensure the context is still valid and we want to catch the error
         * where it has been made
         *
         */

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
            catch (ObjectDisposedException od) // check for context availability : should pass
            {
                // log
            }
        }

        #endregion

        /// <summary>
        /// This region checks for Claim information in the database
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

        /// <summary>
        /// This region removes Claims from the database
        /// </summary>
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
