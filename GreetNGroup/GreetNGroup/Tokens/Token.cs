using System;
using System.Collections.Generic;
using GreetNGroup.Claim_Controls;

namespace GreetNGroup.Tokens
{
    public class Token
    {
        private DateTime assignmentDate;
        private DateTime revokeDate;
        public string UserId { get; set; }
        public List<ClaimsPool.Claims> Claims{ get; set; }
        public string uniqueKey;

        /**
         * Basic constructor for tokens
         *
         * Future implementation may only need id, and will use id to search
         * the related id database for claims linked to said id
         */
        public Token(string id)
        {
            UserId = id;
            assignmentDate = DateTime.Now;
            revokeDate = DateTime.Today.AddDays(1);
            uniqueKey = KeyGen.GenerateRandomCryptoKey(32);
        }
    }
}