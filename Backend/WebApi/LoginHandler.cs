using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ManagerLayer.LoginManagement;
using Newtonsoft.Json;
using ServiceLayer.Requests;

namespace WebApi
{
    class launchResponse
    {
        [Required]
        public string redirectURL { get; set; }
    }

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

            string response = loginMan.Login(ssoRequest);
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
                var returnedRedirectURL = new launchResponse();
                returnedRedirectURL.redirectURL = response;
                var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(returnedRedirectURL))
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }
        }
    }
}