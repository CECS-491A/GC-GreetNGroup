using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Gucci.ServiceLayer.Requests;
using Gucci.ManagerLayer;

namespace WebApi
{
    public class LogOutHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sessionMan = new SessionManager();

            var jsonContent = request.Content.ReadAsStringAsync().Result;
            if (jsonContent == null)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to read request body")
                };
                var tscFail = new TaskCompletionSource<HttpResponseMessage>();
                tscFail.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tscFail.Task;
            }
            SSOUserRequest ssoRequest = new SSOUserRequest();
            ssoRequest = JsonConvert.DeserializeObject<SSOUserRequest>(jsonContent);

            var response = sessionMan.Logout(ssoRequest);

            if (response)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("User has been logged out of GreetNGroup")
                };
                var tscFail = new TaskCompletionSource<HttpResponseMessage>();
                tscFail.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tscFail.Task;
            }

            var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Request is invalid or user doesn't exist in GreetNGroup")
            };
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(httpResponseFail);   // Also sets the task state to "RanToCompletion"
            return tsc.Task;

        }
    }
}