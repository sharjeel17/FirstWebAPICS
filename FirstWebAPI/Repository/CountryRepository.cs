using FirstWebAPI.Data;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) 
        {
            _context = context;
        }

        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CountryExists(string name)
        {
            return _context.Countries.Any(c => c.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public async Task<bool> CreateCountryAsync(Country country)
        {
            _context.Countries.Add(country);
            return await SaveAsync();
        }

        //Also deletes all entries in Owners table that reference countryId
        //(cascading behaviour)
        public async Task<bool> DeleteCountryAsync(int id)
        {
            var country = await _context.Countries.Where(c => c.Id == id).FirstOrDefaultAsync();
            _context.Countries.Remove(country);
            return await SaveAsync();
        }

        public async Task<ICollection<Country>> GetCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country> GetCountryAsync(int id)
        {
            return await _context.Countries.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryByOwnerAsync(int ownerId)
        {
            return await _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersByCountryAsync(int countryId)
        {
            //return _context.Countries.Where(c => c.Id == countryId).Select(c => c.Owners).FirstOrDefault();
            //OR
            return await _context.Owners.Where(o => o.Country.Id == countryId).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            _context.Countries.Update(country);
            return await SaveAsync();
        }
    }
}
