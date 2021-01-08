using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DemoGrpc.Web.Logging
{
    public class LoggerInterceptor : Interceptor
    {
        private readonly ILogger<LoggerInterceptor> _logger;

        public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogCall(context);
            try
            {
                return await continuation(request, context);
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"An SQL error occured when calling {context.Method}");
                Status status;

                if (e.Number == -2)
                {
                    status = new Status(StatusCode.DeadlineExceeded, "SQL timeout");
                }
                else
                {
                    status = new Status(StatusCode.Internal, "SQL error");
                }
                throw new RpcException(status);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured when calling {context.Method}");
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
            
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream, 
            ServerCallContext context, 
            ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(requestStream, context);
        }

        
        public override Task ServerStreamingServerHandler<TRequest, TResponse>(
            TRequest request, 
            IServerStreamWriter<TResponse> responseStream, 
            ServerCallContext context, 
            ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(request, responseStream, context);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream, 
            IServerStreamWriter<TResponse> responseStream, 
            ServerCallContext context, 
            DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(requestStream, responseStream, context);
        }

        private void LogCall(ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            _logger.LogDebug($"Starting call. Request: {httpContext.Request.Path}");
        }
    }
}