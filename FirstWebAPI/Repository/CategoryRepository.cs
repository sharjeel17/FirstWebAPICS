﻿using FirstWebAPI.Data;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;

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

        //return collection of all categories
        public ICollection<Category> GetCategories()
        {
            //can add OrderBy to order by a field/column
            return _context.Categories.ToList();
        }

        //get Category that matches id
        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        //match CategoryId first then return the Pokemon tables from PokemonCategories that match the CategoryId
        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(pc => pc.Pokemon).ToList();
        }
    }
}