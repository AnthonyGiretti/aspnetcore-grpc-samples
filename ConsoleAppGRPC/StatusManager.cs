using Grpc.Core;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ConsoleAppGRPC
{
    public static class StatusManager
    {
        public static StatusCode GetStatusCode(HttpResponseMessage response)
        {
            var headers = response.Headers;
            if (!headers.Contains("grpc-status"))
                return StatusCode.OK;

            return (StatusCode)int.Parse(headers.GetValues("grpc-status").First());
        }
    }
}