using DemoGrpc.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoGrpc.Repository.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAsync();
        Task<Country> GetByIdAsync(int countryId);
        Task<long> AddAsync(Country country);
        Task<int> UpdateAsync(int countryId, Country country);
        Task<int> DeleteAsync(int countryId);
    }
}