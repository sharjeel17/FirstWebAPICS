using FirstWebAPI.Dto;
using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface IPokemonRepository
    {
        //GET READ
        public ICollection<Pokemon> GetPokemons();
        public Pokemon GetPokemon(int id);
        public Pokemon GetPokemon(string name);
        public decimal GetPokemonRating(int pokeId);
        public bool PokemonExists(int pokeId);
        public bool PokemonExists(string name);

        //POST CREATE
        public bool CreatePokemon(int categoryId,int ownerId,Pokemon inputPokemon);
        public bool Save();
    }
}
