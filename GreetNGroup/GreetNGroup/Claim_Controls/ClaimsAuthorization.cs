using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Tokens;
using Microsoft.Ajax.Utilities;

namespace GreetNGroup.Claim_Controls
{
    public class ClaimsAuthorization
    {
        /**
         * Compares list of Claims held within the Token <tok>
         * with required claims in <claimsReq>
         *
         * Except removes all existing recurrences of values in
         * ----> ListName.Except(removeFromList)
         * 
         * Then !ListName.Any() checks if any objects still exist in the list
         *
         * This is used to check if all the claims within the token
         * pass the required claims
         */
        public bool VerifyClaims(Token tok, List<ClaimsPool.Claims> claimsReq)
        {
            var pass = false;
            var currClaims = tok.Claims;
            var claimsCheck = claimsReq.Except(currClaims);
            pass = !claimsCheck.Any();
            return pass;
        }        
    }
}