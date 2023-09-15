using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface ICountryRepository
    {
        //methods in interface are public by default
        //GET READ 
        Task<ICollection<Country>> GetCountriesAsync();
        Task<Country> GetCountryAsync(int id);
        Task<Country> GetCountryByOwnerAsync(int ownerId);
        Task<ICollection<Owner>> GetOwnersByCountryAsync(int countryId);
        bool CountryExists(int id);
        bool CountryExists(string name);

        //POST CREATE
        Task<bool> CreateCountryAsync(Country country);

        //PUT UPDATE
        Task<bool> UpdateCountryAsync(Country country);

        //DELETE
        Task<bool> DeleteCountryAsync(int id);
        Task<bool> SaveAsync();
    }
}
