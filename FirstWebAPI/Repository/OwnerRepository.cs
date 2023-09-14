using FirstWebAPI.Data;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;

namespace FirstWebAPI.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            return Save();
        }

        //Also deletes all entries in PokemonOwner that reference ownerId
        //(cascading)
        public bool DeleteOwner(int ownerId)
        {
            var owner = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
            _context.Owners.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Owner> GetOwnersOfPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(po => po.PokemonId == pokeId).Select(po => po.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        //check if both first name and last name match a name inside database
        public bool OwnerExists(string firstName, string lastName)
        {
            return _context.Owners.Any(o => (o.FirstName.Trim().ToUpper() == firstName.Trim().ToUpper())
            && (o.LastName.Trim().ToUpper() == lastName.Trim().ToUpper()));
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Owners.Update(owner);
            return Save();
        }
    }
}
