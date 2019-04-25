using ManagerLayer.UserManagement;
using Newtonsoft.Json;
using Gucci.ServiceLayer.Requests;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi
{
    public class DeleteHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request, CancellationToken cancellationToken)
        {
            UserManager userMan = new UserManager();

            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            SSOUserRequest ssoRequest = new SSOUserRequest();
            ssoRequest = JsonConvert.DeserializeObject<SSOUserRequest>(jsonContent);

            if (userMan.DeleteUserSSO(ssoRequest))
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("User has been deleted")
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }
            else
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Invalid Signature")
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }
        }
    }
}