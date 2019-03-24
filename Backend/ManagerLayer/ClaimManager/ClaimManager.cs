using System;

namespace ManagerLayer.ClaimManager
{
    public class Class1
    {
        /// <summary>
        /// This region handles inserting claim information into the database
        /// </summary>
        #region Insert Claim Information

        public static void InsertClaim(int claimId, string claimName)
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

        public static bool IsClaimInTable(int claimId)
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
    }
}
