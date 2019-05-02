using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Gucci.ManagerLayer.LoginManagement;
using Newtonsoft.Json;
using Gucci.ServiceLayer.Requests;
using System;

namespace WebApi
{
    public class LoginHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LoginManager loginMan = new LoginManager();

            var content = request.Content;
            var jsonContent = content.ReadAsStringAsync().Result;
            SSOUserRequest ssoRequest = new SSOUserRequest();
            ssoRequest = JsonConvert.DeserializeObject<SSOUserRequest>(jsonContent);

            var response = loginMan.Login(ssoRequest);
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
                var httpResponse = new HttpResponseMessage(HttpStatusCode.Redirect)
                {
                    Content = new StringContent("https://greetngroup.com/login/" + response)
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }
        }
    }
}