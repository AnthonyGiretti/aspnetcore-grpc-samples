using DemoGrpc.Protobufs;
using Grpc.Core;
using System.Threading.Tasks;

namespace DemoGrpc.Web.Services
{
    public class CountryGrpcService : CountryService.CountryServiceBase
    {
        public CountryGrpcService()
        {

        }

        public override async Task<CountriesReply> GetAll(EmptyRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new CountriesReply());
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