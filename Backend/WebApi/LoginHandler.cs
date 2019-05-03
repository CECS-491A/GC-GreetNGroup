using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Gucci.ServiceLayer.Requests;
using System;
using Gucci.ManagerLayer;

namespace WebApi
{
    public class LoginHandler : DelegatingHandler
    {
        private const string redirectBaseUrl = "https://greetngroup.com/login/?token=";

        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var jsonContent = request.Content.ReadAsStringAsync().Result;
                if (jsonContent == null)
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Unable to read request body")
                    };
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                    return tsc.Task;
                }
                SSOUserRequest ssoRequest = new SSOUserRequest();
                ssoRequest = JsonConvert.DeserializeObject<SSOUserRequest>(jsonContent);

                var sessionMan = new SessionManager();
                var response = sessionMan.Login(ssoRequest);
                if (response == "-1")
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Invalid Session")
                    };
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                    return tsc.Task;
                }
                else
                {
                    var redirect = request.CreateResponse(HttpStatusCode.SeeOther);
                    redirect.Headers.Location = new Uri(redirectBaseUrl + response);
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(redirect);   // Also sets the task state to "RanToCompletion"
                    return tsc.Task;
                }
            }
            catch (Exception e)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to login at this time" + e)
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }
        }
    }
}