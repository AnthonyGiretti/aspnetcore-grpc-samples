using Calzolari.Grpc.Net.Client.Validation;
using DemoGrpc.Domain.Entities;
using DemoGrpc.Protobufs;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using System.Net;
using DemoGrpc.Protobufs.V1;
using Microsoft.Extensions.Logging;
using static DemoGrpc.Protobufs.V1.CountryService;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text;

namespace ConsoleAppGRPC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // DI
            var services = new ServiceCollection();

            var loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            });

            var serverErrors = new HttpStatusCode[] { 
                HttpStatusCode.BadGateway, 
                HttpStatusCode.GatewayTimeout, 
                HttpStatusCode.ServiceUnavailable, 
                HttpStatusCode.InternalServerError, 
                HttpStatusCode.TooManyRequests, 
                HttpStatusCode.RequestTimeout 
            };

            var gRpcErrors = new StatusCode[] {
                StatusCode.DeadlineExceeded,
                StatusCode.Internal,
                StatusCode.NotFound,
                StatusCode.ResourceExhausted,
                StatusCode.Unavailable,
                StatusCode.Unknown
            };

            Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> retryFunc = (request) =>
            {
                return Policy.HandleResult<HttpResponseMessage>(r => {
                    
                    var grpcStatus = StatusManager.GetStatusCode(r);
                    var httpStatusCode = r.StatusCode;

                    return (grpcStatus == null && serverErrors.Contains(httpStatusCode)) || // if the server send an error before gRPC pipeline
                           (httpStatusCode == HttpStatusCode.OK && gRpcErrors.Contains(grpcStatus.Value)); // if gRPC pipeline handled the request (gRPC always answers OK)
                })
                .WaitAndRetryAsync(3, (input) => TimeSpan.FromSeconds(3 + input), (result, timeSpan, retryCount, context) =>
                                    {
                                        var grpcStatus = StatusManager.GetStatusCode(result.Result);
                                        Console.WriteLine($"Request failed with {grpcStatus}. Retry");
                                    });
            };


            // https://grpcwebdemo.azurewebsites.net
            // gRPC
            services.AddGrpcClient<CountryServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:5001");
            }).AddPolicyHandler(retryFunc);
            var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<CountryServiceClient>();

            /*
            // https://grpcwebdemo.azurewebsites.net
            // gRPC-Web
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler),
                LoggerFactory = loggerFactory
            });
            var clientWeb = new CountryServiceClient(channel);
            */


            try
            {
                //// Get all gRPC
                //var countries = (await client.GetAllAsync(new EmptyRequest())).Countries.Select(x => new Country
                //{
                //    CountryId = x.Id,
                //    Description = x.Description,
                //    CountryName = x.Name
                //}).ToList();

                //Console.WriteLine("Found countries");
                //countries.ForEach(x => Console.WriteLine($"Found country {x.CountryName} ({x.CountryId}) {x.Description}"));

                //Console.WriteLine();
                //Console.WriteLine();

                //// Get all gRPC - web
                //var countriesweb = (await clientWeb.GetAllAsync(new EmptyRequest())).Countries.Select(x => new Country
                //{
                //    CountryId = x.Id,
                //    Description = x.Description,
                //    CountryName = x.Name
                //}).ToList();

                //Console.WriteLine("Found countries with gRPC-Web");
                //countriesweb.ForEach(x => Console.WriteLine($"Found country with gRPC-Web:  {x.CountryName} ({x.CountryId}) {x.Description}"));

                // Create 
                var createdCountry = await client.CreateAsync(new CountryCreateRequest { Name = "Japan", Description = "test" });

                //var createdCountry2 = await clientWeb.CreateAsync(new CountryCreateRequest { Name = "Japan", Description = "" });
            }
            catch (RpcException e)
            {
                var errors = e.GetValidationErrors(); // Gets validation errors list
                Console.WriteLine(e.Message);
            }
        }

    }
}