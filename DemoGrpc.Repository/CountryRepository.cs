using DemoGrpc.Domain.Entities;
using DemoGrpc.Repository.Database;
using DemoGrpc.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoGrpc.Repository
{
    public class EFCountryRepository : ICountryRepository
    {
        private readonly DemoDbContext _dbContext;
        public EFCountryRepository(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Country>> GetAsync()
        {
            return await _dbContext.Country.AsNoTracking().ToListAsync();
        }

        public async Task<Country> GetByIdAsync(int countryId)
        {
            return await _dbContext.Country.AsNoTracking().FirstOrDefaultAsync(x => x.CountryId == countryId);
        }

        public async Task<Country> AddAsync(Country country)
        {
            _dbContext.Add(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        public async Task<int> UpdateAsync(Country country)
        {
            var countryToUpdate = await _dbContext.Country
                                                    .Where(x => x.CountryId == country.CountryId)
                                                    .FirstOrDefaultAsync();

            countryToUpdate.CountryName = country.CountryName;
            countryToUpdate.Description = country.Description;

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int countryId)
        {
            var countryToDelete = await _dbContext.Country
                                                    .Where(x => x.CountryId == countryId)
                                                    .FirstOrDefaultAsync();

            _dbContext.Remove(countryToDelete);
            return await _dbContext.SaveChangesAsync();
        }
    }
}