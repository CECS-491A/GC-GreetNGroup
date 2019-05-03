using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi
{
    public class JWTHandler : DelegatingHandler
    {
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

                var _jwtService = new JWTService();

                TokenRequest tknRequest = new TokenRequest();
                tknRequest = JsonConvert.DeserializeObject<TokenRequest>(jsonContent);

                var response = _jwtService.IsTokenValid(tknRequest.token);

                if (response)
                {
                    var newJwtToken = _jwtService.RefreshToken(tknRequest.token);

                    var httpResponseSuccess = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(newJwtToken)
                    };
                    var tscSuccess = new TaskCompletionSource<HttpResponseMessage>();
                    tscSuccess.SetResult(httpResponseSuccess);   // Also sets the task state to "RanToCompletion"
                    return tscSuccess.Task;
                }
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Token is invalid")
                };
                var tscFail = new TaskCompletionSource<HttpResponseMessage>();
                tscFail.SetResult(httpResponseFail);   // Also sets the task state to "RanToCompletion"
                return tscFail.Task;

            }
            catch (Exception e)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to validate JWTToken at this time" + e)
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }
        }
    }
}