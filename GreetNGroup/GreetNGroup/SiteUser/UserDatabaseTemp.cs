using System.Collections.Generic;
using GreetNGroup.Claim_Controls;

namespace GreetNGroup.SiteUser
{
    public class UserDatabaseTemp
    {
        /// <summary>
        ///
        /// The following creates a Dictionary to represent a database of users
        /// The functionality to be tested is the ability to retrieve claims
        /// through referencing the userID
        ///
        /// </summary>
        #region Table Setup
        private Dictionary<string, List<ClaimsPool.Claims>> userTable =
            new Dictionary<string, List<ClaimsPool.Claims>>();

        public void SetupTable()
        {
            userTable.Add("19452746", new List<ClaimsPool.Claims>
            {
                ClaimsPool.Claims.CanCreateEvents,
                ClaimsPool.Claims.CanViewEvents,
                ClaimsPool.Claims.Over18,
                ClaimsPool.Claims.AdminRights
            });
            userTable.Add("10294753", new List<ClaimsPool.Claims>
            {
                ClaimsPool.Claims.CanBlacklistUsers,
                ClaimsPool.Claims.Over18
            });
            userTable.Add("45892987", new List<ClaimsPool.Claims>
            {
                ClaimsPool.Claims.CanCreateEvents,
                ClaimsPool.Claims.CanViewEvents,
                ClaimsPool.Claims.CanFriendUsers,
                ClaimsPool.Claims.Over18
            });
            userTable.Add("88304271", new List<ClaimsPool.Claims>
            {
                ClaimsPool.Claims.CanCreateEvents,
                ClaimsPool.Claims.CanViewEvents,
                ClaimsPool.Claims.Over18
            });
        }
#endregion
        
        // This will send the created dictionary back
        public Dictionary<string, List<ClaimsPool.Claims>> ReturnUserTable()
        {
            return userTable;
        }
    }
}