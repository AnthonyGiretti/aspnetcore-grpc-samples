using DemoGrpc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DempGrpc.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetAsync();
        Task<Country> GetByIdAsync(int countryId);
        Task<Country> AddAsync(Country country);
        Task<Country> UpdateAsync(Country country);
        Task<bool> DeleteAsync(int countryId);
    }
}