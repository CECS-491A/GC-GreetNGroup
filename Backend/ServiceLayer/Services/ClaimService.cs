using System;
using System.Linq;
using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ServiceLayer.Services
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
        private ILoggerService _gngLoggerService;

        public ClaimService()
        {
            _gngLoggerService = new LoggerService();
        }
        /// <summary>
        /// This region handles inserting claim information into the database
        /// </summary>
        #region Insert Claim Information

        // Inserts claim with corresponding claim id into database
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
            }
        }

        #endregion

        /// <summary>
        /// This region checks for Claim information in the database
        /// </summary>
        #region Claim Information Check

        // Checks if claim exists within the database given claimId
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion

        /// <summary>
        /// This region removes Claims from the database
        /// </summary>
        #region Delete Claim Information

        // Removes claim from database given proper claimId
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
            }
        }

        #endregion
    }
}
