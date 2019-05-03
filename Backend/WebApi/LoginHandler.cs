using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Gucci.ServiceLayer.Requests;
using System;
using Gucci.ManagerLayer;
using System.IO;

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
                var body = string.Empty;
                using(var reader = new StreamReader(request.Content.ReadAsStreamAsync().Result))
                {
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    body = reader.ReadToEnd();
                }
                if (body == null)
                {
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Unable to read request body")
                    };
                    var tscFail = new TaskCompletionSource<HttpResponseMessage>();
                    tscFail.SetResult(httpResponseFail);   // Also sets the task state to "RanToCompletion"
                    return tscFail.Task;
                }
                SSOUserRequest ssoRequest = new SSOUserRequest();
                ssoRequest = JsonConvert.DeserializeObject<SSOUserRequest>(body);

                Console.WriteLine(ssoRequest.ToString());

                var sessionMan = new SessionManager();
                /*
                var response = sessionMan.Login(ssoRequest);
                
                if (response == "-1")
                {
                    
                }
                else
                {
                    var redirect = request.CreateResponse(HttpStatusCode.SeeOther);
                    redirect.Headers.Location = new Uri(redirectBaseUrl + response);
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(redirect);   // Also sets the task state to "RanToCompletion"
                    return tsc.Task;
                }
                */
                var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Invalid Session")
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
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