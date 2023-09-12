using FirstWebAPI.Data;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;

namespace FirstWebAPI.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int categoryId, int ownerId, Pokemon pokemon)
        {
            Console.WriteLine(pokemon.Id);
            _context.Pokemon.Add(pokemon);
            if (!Save()) return false;
            Console.WriteLine(pokemon.Id);

            var owner = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokeOwner = new PokemonOwner
            {
                Pokemon = pokemon,
                Owner = owner
            };
            _context.PokemonOwners.Add(pokeOwner);

            var pokeCategory = new PokemonCategory
            {
                Pokemon = pokemon,
                Category = category
            };

            _context.PokemonCategories.Add(pokeCategory);
            return Save();

        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(r => r.Pokemon.Id == pokeId);

            if (review.Count() <= 0) return 0;

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool PokemonExists(string name)
        {
            return _context.Pokemon.Any(p => p.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
