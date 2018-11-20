using Microsoft.Ajax.Utilities;

namespace GreetNGroup.Claim_Controls
{
    public class ClaimsAuthorization
    {
        // will take reference to user/user token to check relevant claims, returns pass or fail
        public bool VerifyClaims(Var userRef, ClaimsPool.Claims[] claimsReq)
        {
            for (int i = 0; i < claimsReq.Length; i++)
            {
                // check all claims within the userRef with that of the claimsReq
                // if at least one does not match the claimsReq return false
            }
            return true;
        }        
    }
}