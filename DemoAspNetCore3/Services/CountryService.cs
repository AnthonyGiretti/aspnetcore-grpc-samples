using AutoMapper;
using DemoGrpc.Domain.Entities;
using DemoGrpc.Protobufs;
using DempGrpc.Services.Interfaces;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace DemoGrpc.Web.Services
{
    public class CountryGrpcService : CountryService.CountryServiceBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountryGrpcService(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        //[Authorize]
        public override async Task<CountriesReply> GetAll(EmptyRequest request, ServerCallContext context)
        {
            try
            {
                var countries = await _countryService.GetAsync();
                return _mapper.Map<CountriesReply>(countries);
            }
            catch (Exception e)
            {
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
        }

        //[Authorize]
        public override async Task<CountryReply> GetById(CountrySearchRequest request, ServerCallContext context)
        {
            var currentUser = context.GetHttpContext().User;
            try
            {
                var country = await _countryService.GetByIdAsync(request.CountryId);
                return _mapper.Map<CountryReply>(country);
            }
            catch (Exception e)
            {
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
        }

        //[Authorize]
        public override async Task<CountryReply> Create(CountryCreateRequest request, ServerCallContext context)
        {
            //var currentUser = context.GetHttpContext().User;
            //throw new RpcException(new Status(StatusCode.InvalidArgument,"test"), "test");
            try
            {
                var createCountry = _mapper.Map<Country>(request);
                var country = await _countryService.AddAsync(createCountry);
                return _mapper.Map<CountryReply>(country);
            }
            catch (Exception e)
            {
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
        }

        //[Authorize]
        public override async Task<CountryReply> Update(CountryRequest request, ServerCallContext context)
        {
            var currentUser = context.GetHttpContext().User;
            try
            {
                var updateCountry = _mapper.Map<Country>(request);
                var country = await _countryService.UpdateAsync(updateCountry);
                return _mapper.Map<CountryReply>(country);
            }
            catch (Exception e)
            {
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
        }

        //[Authorize]
        public override async Task<EmptyReply> Delete(CountrySearchRequest request, ServerCallContext context)
        {
            var currentUser = context.GetHttpContext().User;
            try
            {
                await _countryService.DeleteAsync(request.CountryId);
                return new EmptyReply();
            }
            catch (Exception e)
            {
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
        }
    }
}