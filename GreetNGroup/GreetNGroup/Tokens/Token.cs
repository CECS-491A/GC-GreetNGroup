using System;
using System.Collections.Generic;
namespace GreetNGroup.Tokens
{
    public class Token
    {
        private DateTime revokeDate;
        public int UserId { get; set; }
        public List<int> ClaimIds{ get; set; }
        public string uniqueKey;
        
        /// <summary>
        /// Creates a token holding user information and information
        /// unique to the token like its revoke date
        /// </summary>
        /// <param name="id"> user ID to reference the user with </param>
        public Token(int id)
        {
            UserId = id;
            //ClaimIds = DataBaseQueries.ListUserClaims(id);
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