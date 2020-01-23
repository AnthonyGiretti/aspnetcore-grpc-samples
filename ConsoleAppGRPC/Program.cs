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
                return Policy.HandleResult<HttpResponseMessage>(r => r.Headers.GetValues("grpc-status").FirstOrDefault() != "0")
                             .WaitAndRetryAsync(3, (input) => TimeSpan.FromSeconds(3 + input), (result, timeSpan, retryCount, context) =>
                                                    {
                                                        var grpcStatus = (StatusCode)int.Parse(result.Result.Headers.GetValues("grpc-status").FirstOrDefault());
                                                        Console.WriteLine($"Request failed with {grpcStatus}. Retry");
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
                
                // Create 
                var createdCountry = await client.CreateAsync(new CountryCreateRequest { Name = "Japan", Description = "Rising sun country" }); // Remove Name or Description to test validation
                var country = new Country
                {
                    CountryId = createdCountry.Id,
                    CountryName = createdCountry.Name,
                    Description = createdCountry.Description
                };
                Console.WriteLine($"Country {country.CountryName} ({country.CountryId}) created!");

                // GetById
                var foundCountry = await client.GetByIdAsync(new CountrySearchRequest { CountryId = country.CountryId });
                country = new Country
                {
                    CountryId = foundCountry.Id,
                    CountryName = foundCountry.Name,
                    Description = foundCountry.Description
                };
                Console.WriteLine($"Found country {country.CountryName} ({country.CountryId})");

                // Update 
                var updatedCountry = await client.UpdateAsync(new CountryRequest { Id = country.CountryId, Name = "Japan", Description = "rising sun country, Nippon!!!" });
                country = new Country
                {
                    CountryId = updatedCountry.Id,
                    CountryName = updatedCountry.Name,
                    Description = updatedCountry.Description
                };
                Console.WriteLine($"Country {country.CountryName} ({country.CountryId}) updated with new description: {country.Description}");

                // Delete
                await client.DeleteAsync(new CountrySearchRequest { CountryId = country.CountryId });
                Console.WriteLine($"Deleted country {country.CountryName} ({country.CountryId})");

                // Get all
                var countries = (await client.GetAllAsync(new EmptyRequest())).Countries.Select(x => new Country
                {
                    CountryId = x.Id,
                    Description = x.Description,
                    CountryName = x.Name
                }).ToList();

                Console.WriteLine("Found countries");
                countries.ForEach(x => Console.WriteLine($"Found country {x.CountryName} ({x.CountryId}) {x.Description}"));
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}