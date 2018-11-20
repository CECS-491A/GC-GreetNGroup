using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken tok)
        {
            /*
             *      The following line reads in the user from the current http request
             *      presented by the HttpActionContext argument
             *      and retrieves the claims principle from that user
             *
             *      actionContext.RequestContext.Principal represents the user
             *      ClaimsPrinciple is used to allow/give us access to the user's claims
             */
            ClaimsPrincipal claims = actionContext.RequestContext.Principal as ClaimsPrincipal;
            
            if (claims != null)
            {
                /*
                 * 1. Check for authentication of user
                 *
                 * Accesses the ClaimsPrincipal Identity and Checks for authentication
                 */
                if (!claims.Identity.IsAuthenticated)
                {
                    /*
                     * User is not authenticated, Unauthorized message is returned
                     */
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return Task.FromResult<object>(null);
                }

                /*
                 * 2. Check for claims
                 * 
                 * Lambda function used in ClaimsPrincipal.HasClaim function to check whether
                 * or not the current user has the correct claim or claimValue to perform
                 * specific action
                 */
                if (!claims.HasClaim(x => x.Type == Claim && x.Value == ClaimValue))
                {
                    /*
                     * Claims on current user do not meet required claims for authorization
                     */
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return Task.FromResult<object>(null);
                }
            }
            else
            {
                /*
                 * User claims does not exist in this context
                 */
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
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