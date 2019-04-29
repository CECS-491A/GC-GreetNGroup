using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Gucci.ManagerLayer.LoginManagement;
using Newtonsoft.Json;
using Gucci.ServiceLayer.Requests;

namespace WebApi
{
    public class LogOutHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("User must log out on GreetNGroup")
            };
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
            return tsc.Task;
        }
    }
}