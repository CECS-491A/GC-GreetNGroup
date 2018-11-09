using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GreetNGroup.Filters
{
    // setup to retrieve and test for claims
    public class ClaimsAuthorizationFilter : AuthorizationFilterAttribute
    {
        /*
         * Allows us to grab claims and claim values as needed
         */
        public String Claim { get; set; }
        public String ClaimValue { get; set; }

        /*
         * Overrides function OnAuthorizationAsync(HttpActionContext, CancellationToken)
         *
         * It is called when a call requires authorization and is asynchronous 
         */
        public override Task OnAuthorizationAsync(HttpActionContext action, CancellationToken tok)
        {
            /*
             * Goals - 
             *   1: check if the current user making a request is authenticated
             *   2: if the user is authenticated --check claims allowed for this user
             *
             *      The following line reads in the user from the current http request
             *      presented by the HttpActionContext argument
             *      and retrieves the claims principle from that user
             */
            ClaimsPrincipal claims = action.RequestContext.Principal as ClaimsPrincipal;

            /*
             * Lambda function used in ClaimsPrincipal.HasClaim function to check whether
             * or not the current user has the correct claim or claimValue to perform
             * specific action
             */
            if (claims.HasClaim(x => x.Type == Claim && x.Value == ClaimValue))
            {

            }

            /*
             * To Return a task here for an asynchronous function, there must be no
             * stalls or wait. Therefore, as a no-operation way to return,
             *
             * Task.FromResult<object>(null)
             *
             * is returned, to quickly create and return a type of Task
             */
            return Task.FromResult<object>(null);
        }
    }
}