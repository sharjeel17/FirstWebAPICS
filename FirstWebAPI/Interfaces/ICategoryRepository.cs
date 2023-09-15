using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface ICategoryRepository
    {
        /* GET/READ */
        public Task<ICollection<Category>> GetCategoriesAsync();
        public Task<Category> GetCategoryAsync(int id);
        public Task<ICollection<Pokemon>> GetPokemonByCategoryAsync(int categoryId);
        public bool CategoryExists(int id);
        public bool CategoryExists(string name);

        /* POST/CREATE */
        public Task<bool> CreateCategoryAsync(Category category);

        //UPDATE PUT
        public Task<bool> UpdateCategoryAsync(Category category);

        //DELETE 
        public Task<bool> DeleteCategoryAsync(int id);

        public Task<bool> SaveAsync();

        
    }
}
