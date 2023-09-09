using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Interfaces;
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
        public IActionResult GetCategories() 
        {
            var categories = _mapper.Map<ICollection<CategoryDto>>(_categoryRepository.GetCategories());

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(categories);
        }
        
    }
}
