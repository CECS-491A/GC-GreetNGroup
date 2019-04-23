using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApi
{
    public class HealthHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Good Health")
            };
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
            return tsc.Task;

            /*
            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Under Maintenance")
            };
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
            return tsc.Task;
            */
        }
    }
}