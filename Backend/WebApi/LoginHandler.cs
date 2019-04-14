using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ManagerLayer.LoginManagement;
using Newtonsoft.Json;
using ServiceLayer.Requests;

namespace WebApi
{
    public class LoginHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LoginManager lm = new LoginManager();

            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            SSOUserRequest ssoRequest = new SSOUserRequest();
            ssoRequest = JsonConvert.DeserializeObject<SSOUserRequest>(jsonContent);

            string response = lm.Login(ssoRequest);
            if (response.Equals("-1"))
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
                    Content = new StringContent(response)
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }
        }
    }
}