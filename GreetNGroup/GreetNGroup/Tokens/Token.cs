using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GreetNGroup.Claim_Controls;
using GreetNGroup.User;

namespace GreetNGroup.Tokens
{
    public class Token
    {
        private DateTime assignmentDate;
        private DateTime revokeDate;
        public string UserId { get; set; }
        public List<ClaimsPool.Claims> Claims{ get; set; }
        public string uniqueKey;
        
        public Token(string id)
        {
            UserId = id;
            assignmentDate = DateTime.Now;
            revokeDate = DateTime.Today.AddDays(1);
            /*
             * Use the uniqueKey generated here as the key to hashing the value
             * representative of this token, this key should be held in a .pem file
             * in our project, and will not be held by the token itself
             *
             * The hashed value that is made will be held by this token
             * and when the token is sent back to the server to check, only for some functions,
             * it will check to see if the hash has been changed, to verify weather or not
             * the user has done something to the token.
             *
             * This will not happen on every pass, to lessen the load on the server, by making it so that
             * the front end does not need to constantly talk to the backend
             *
             * This is because, in general, the token has hold of the claims that needs to be checked,
             * thus it does not need to contact the backend in most situations
             *
             * 
             */
            uniqueKey = KeyGen.GenerateRandomCryptoKey(32);
        }
    }
}