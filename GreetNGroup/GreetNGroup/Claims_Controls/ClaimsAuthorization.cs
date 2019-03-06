using System;
using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Tokens;
using Microsoft.Ajax.Utilities;

namespace GreetNGroup.Claim_Controls
{
    public static class ClaimsAuthorization
    {
        /// <summary>
        /// This function compares claims located on the token with the claims required
        /// to perform the action requested by the user
        /// </summary>
        /// <param name="tok"> token </param>
        /// <param name="claimsReq"> required claims </param>
        /// <returns></returns>
        public static bool VerifyClaims(Token tok, List<int> claimsReq)
        {
            try
            {
                var pass = false;
                var currClaims = tok.ClaimIds;
                var claimsCheck = claimsReq.Except(currClaims);
                pass = !claimsCheck.Any();
                return pass;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }        
    }
}