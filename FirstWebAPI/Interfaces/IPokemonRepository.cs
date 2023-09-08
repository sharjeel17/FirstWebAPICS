using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface IPokemonRepository
    {
        public ICollection<Pokemon> GetPokemons();
        public Pokemon GetPokemon(int id);
        public Pokemon GetPokemon(string name);
        public decimal GetPokemonRating(int pokeId);
        public bool PokemonExists(int pokeId);
    }
}
