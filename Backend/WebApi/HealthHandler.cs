using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gucci.DataAccessLayer.Context;

namespace WebApi
{
    public class HealthHandler : DelegatingHandler
    {
        private const string awsInstanceIPAddress = "54.193.2.190";
        private const int timeout = 12000; // 12 seconds
        private const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var _db = new GreetNGroupContext())
            {
                try
                {
                    var existingConnection = _db.Database.Exists();
                    if (!existingConnection)
                    {
                        var httpResponseError = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("GreetNGroup is offline, database connection error")
                        };
                        var tscError = new TaskCompletionSource<HttpResponseMessage>();
                        tscError.SetResult(httpResponseError);   // Also sets the task state to "RanToCompletion"
                        return tscError.Task;
                    }
                    _db.SaveChanges();

                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions
                    {
                        // Use the default Ttl value which is 128,
                        // but change the fragmentation behavior.
                        DontFragment = true
                    };


                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    PingReply reply = pingSender.Send(awsInstanceIPAddress, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        var httpResponseSuccess = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("GreetNGroup is online")
                        };
                        var tscSuccess = new TaskCompletionSource<HttpResponseMessage>();
                        tscSuccess.SetResult(httpResponseSuccess);   // Also sets the task state to "RanToCompletion"
                        return tscSuccess.Task;
                    }

                    var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("GreetNGroup is offline")
                    };
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                    return tsc.Task;

                }
                catch (Exception) // catch error when trying to call db, return status of internal problems
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("GreetNGroup is offline")
                    };
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                    return tsc.Task;
                }
            }
        }
    }
}