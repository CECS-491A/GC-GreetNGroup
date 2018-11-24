using System.Collections.Generic;
using GreetNGroup.Claim_Controls;

namespace GreetNGroup.Tokens
{
    public class Token
    {
        public string UserId { get; set; }
        public List<ClaimsPool.Claims> Claims { get; set; }

        /**
         * Basic constructor for tokens
         *
         * Future implementation may only need id, and will use id to search
         * the related id database for claims linked to said id
         */
        public Token(string id, List<ClaimsPool.Claims> claimList)
        {
            UserId = id;
            Claims = claimList;
        }
    }
}