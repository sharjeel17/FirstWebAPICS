using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface ICountryRepository
    {
        //methods in interface are public by default
        //GET READ 
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersByCountry(int countryId);
        bool CountryExists(int id);
        bool CountryExists(string name);

        //POST CREATE
        bool CreateCountry(Country country);
        bool Save();
    }
}
