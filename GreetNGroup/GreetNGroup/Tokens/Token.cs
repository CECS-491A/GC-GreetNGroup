using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Data_Access;
using GreetNGroup.SiteUser;

namespace GreetNGroup.Tokens
{
    public class Token
    {
        private DateTime revokeDate;
        public string UserId { get; set; }
        public List<string> Claims{ get; set; }
        public string uniqueKey;
        
        /// <summary>
        /// Creates a token holding user information and information
        /// unique to the token like its revoke date
        /// </summary>
        /// <param name="id"> user ID to reference the user with </param>
        public Token(string id)
        {
            UserId = id;
            Claims = DataBaseQueries.ListUserClaims(id);
            revokeDate = DateTime.Today.AddDays(1);
            /*
             * The unique key here is temporary -- will later be used for
             * hashing the token before it is given to the user
             * The key itself will not be stored here
             */
            uniqueKey = KeyGen.GenerateRandomCryptoKey(32);
        }
    }
}