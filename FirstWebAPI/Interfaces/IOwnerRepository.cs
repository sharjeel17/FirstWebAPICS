using FirstWebAPI.Dto;
using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface IOwnerRepository
    {
        //READ GET
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnersOfPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonsByOwner(int ownerId);
        bool OwnerExists(int ownerId);
        bool OwnerExists(string firstName, string lastName);

        //CREATE POST
        bool CreateOwner(Owner owner);
        bool Save();
        
    }
}
