using DemoGrpc.Domain.Entities;
using DemoGrpc.Protobufs;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static DemoGrpc.Protobufs.CountryService;

namespace ConsoleAppGRPC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // DI
            var services = new ServiceCollection();

            Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> retryFunc = (request) =>
            {
                return HttpPolicyExtensions.HandleTransientHttpError()
                                .WaitAndRetryAsync(3, (input) => TimeSpan.FromSeconds(3 + input), (result, timeSpan, retryCount, context) =>
                                                    {
                                                        Console.Write($"Request failed with {result.Result.StatusCode}.\n");
                                                    });
            };


            services.AddGrpcClient<CountryServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:5001");
            }).AddPolicyHandler(retryFunc);

            var provider = services.BuildServiceProvider();

            var client = provider.GetRequiredService<CountryServiceClient>();
            
            try
            {
                var countries = (await client.GetAllAsync(new EmptyRequest())).Countries.Select(x => new Country
                {
                    CountryId = x.Id,
                    Description = x.Description,
                    CountryName = x.Name
                }).ToList();
            }
            catch (RpcException e)
            {

            }
        }
    }
}