using DemoGrpc.Protobufs;
using DempGrpc.Services.Interfaces;
using Grpc.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoGrpc.Web.Services
{
    public class CountryGrpcService : CountryService.CountryServiceBase
    {
        private readonly ICountryService _countryService;

        public CountryGrpcService(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public override async Task<CountriesReply> GetAll(EmptyRequest request, ServerCallContext context)
        {
            try
            {
                var countries = await _countryService.GetAsync();

                return new CountriesReply
                {
                    Countries = { countries.Select(x => new CountryReply
                    {
                        Id = x.CountryId,
                        Description = x.Description,
                        Name = x.CountryName
                    }) }
                };
            }
            catch (Exception e)
            {
                var httpContext = context.GetHttpContext();
                httpContext.Response.StatusCode = 500;
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
        }

        public override async Task<CountryReply> GetById(CountrySearchRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new CountryReply());
        }

        public override async Task<CountryReply> Create(CountryCreateRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new CountryReply());
        }

        public override async Task<CountryReply> Update(CountryRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new CountryReply());
        }

        public override async Task<EmptyReply> Delete(CountrySearchRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new EmptyReply());
        }
    }
}