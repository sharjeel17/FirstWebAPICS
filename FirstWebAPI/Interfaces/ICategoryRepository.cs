using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface ICategoryRepository
    {
        /* GET/READ */
        public ICollection<Category> GetCategories();
        public Category GetCategory(int id);
        public ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        public bool CategoryExists(int id);
        public bool CategoryExists(string name);

        /* POST/CREATE */
        public bool CreateCategory(Category category);
        public bool Save();

        //UPDATE PUT
        public bool UpdateCategory(Category category);
    }
}
