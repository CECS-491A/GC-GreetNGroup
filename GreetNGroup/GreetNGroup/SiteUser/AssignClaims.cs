using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Tokens;

namespace GreetNGroup.SiteUser
{
    public class AssignClaims
    {
        /// <summary>
        /// Function for system admins to add claims to a user, system admins can add any type of claim
        /// to any user
        /// </summary>
        /// <param name="editor"></param>
        /// Holds reference to user information of the user trying to edit
        /// 
        /// <param name="claim"></param>
        /// Holds the claim that the user wants to add
        /// 
        /// <param name="userAcc"></param>
        /// Holds the reference to the user that the claim should be added to
        public void AssignClaimToUser(Token editor, ClaimsPool.Claims claim , UserAccount userAcc)
        {
            List<ClaimsPool.Claims> systemAdminExclude = new List<ClaimsPool.Claims>(){ClaimsPool.Claims.SystemAdmin};
            
            if (ClaimsAuthorization.VerifyClaims(editor, systemAdminExclude))
            {
                userAcc.addClaim(claim);
            }
            // add return for false here
        }
    }
}