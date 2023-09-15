using FirstWebAPI.Data;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        //constructor to inject DataContext (database) in to the category repository
        public CategoryRepository(DataContext context)
        {
            _context = context;
            
        }

        //returns whether Category exists or not
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CategoryExists(string name)
        {
            return _context.Categories.Any(c => c.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }


        public async Task<bool> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            return await SaveAsync();
        }

        //IMPORTANT
        //When a Category (which is a Foreign Key in the PokemonCategories Join-Table) is deleted
        //all of the enteries with categoryId inside of the Join-Table are also deleted
        //(Called Cascading)
        //Cascading is enabled by default
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
            _context.Categories.Remove(category);
            return await SaveAsync();
        }

        //return collection of all categories
        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            //can add OrderBy to order by a field/column
            return await _context.Categories.ToListAsync();
        }

        //get Category that matches id
        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        //match CategoryId first then return the Pokemon tables from PokemonCategories that match the CategoryId
        public async Task<ICollection<Pokemon>> GetPokemonByCategoryAsync(int categoryId)
        {
            return await _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(pc => pc.Pokemon).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            return await SaveAsync();
        }
    }
}
