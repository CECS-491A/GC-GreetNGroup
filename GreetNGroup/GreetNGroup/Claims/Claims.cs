using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GreetNGroup.Data_Access;
using GreetNGroup.Tokens;

namespace GreetNGroup.Claims
{
    public class Claims
    {
        private List<string> usersClaims = null;
        public Claims(string userID)
        {
            usersClaims = DataBaseQueries.ListUserClaims(userID);
        }
    }
}