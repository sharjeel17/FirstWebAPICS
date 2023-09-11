using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) 
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCategories() 
        {
            var categories = _mapper.Map<ICollection<CategoryDto>>(_categoryRepository.GetCategories());

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!(_categoryRepository.CategoryExists(categoryId))) return NotFound();

            var categories = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonByCategory(int categoryId) 
        {
            if (!(_categoryRepository.CategoryExists(categoryId))) return NotFound();

            var pokemon = _mapper.Map<ICollection<PokemonDto>>(_categoryRepository.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        //FromBody attribute ensures that the category key:value pairs
        //only come from the body and nowhere else
        public IActionResult CreateCategory([FromBody] CategoryDto inputCategory) 
        {
            //not needed/redundant as ModelState itself will do the validation check
            //before ever executing code in this method
            //statement written for safety and housekeeping
            if (inputCategory == null) return BadRequest(ModelState);

            //EF will check and see if there are matching ID/Primary keys
            //at the createCategory and throw error if there is
            //but better to do self checking and returning early to reduce errors
            if (_categoryRepository.CategoryExists(inputCategory.Id))
                ModelState.AddModelError("ID Error", "ID Key already exists");

            //check for matching category names with the given input
            //if category name already exists then add an error to the ModelState
            //which in turn will turn the ModelState invalid
            if (_categoryRepository.CategoryExists(inputCategory.Name)) 
                ModelState.AddModelError("Name Error", "Category already exists");

            //if any above conditions are met then ModelState will be invalid
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Identity_Insert is off, so Id needs to be null
            //as we cannot set Id value ourselves.
            //If Identity_Insert is on, comment this line out
            inputCategory.Id = null;

            //map CategoryDto to Category
            var categoryMap = _mapper.Map<Category>(inputCategory);

            //Add mapped Category object to database and check if successful
            if (!_categoryRepository.CreateCategory(categoryMap)) 
            {
                ModelState.AddModelError("Create Error", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
