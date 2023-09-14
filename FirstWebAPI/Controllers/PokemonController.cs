using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, ICategoryRepository categoryRepository,
            IOwnerRepository ownerRepository ,IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _categoryRepository = categoryRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<ICollection<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(int id) 
        {
            //if the id is not found then return not found status
            if (!(_pokemonRepository.PokemonExists(id))) return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonRating(int id) 
        {
            if (!(_pokemonRepository.PokemonExists(id))) return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreatePokemon([FromBody] CreatePokemonDto inputPokemon) 
        {
            if (inputPokemon == null) 
                return BadRequest(ModelState);

            if(_pokemonRepository.PokemonExists(inputPokemon.Name))
                ModelState.AddModelError("Pokemon Exists", "Entered name of Pokemon already exists");

            if (!_categoryRepository.CategoryExists(inputPokemon.categoryId))
                ModelState.AddModelError("Bad CategoryId", "Please enter a valid category ID");

            if (!_ownerRepository.OwnerExists(inputPokemon.ownerId))
                ModelState.AddModelError("Bad OwnerId", "Please enter a valid Owner ID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Unlike other controllers (Category, Country and Owner)
            //I'm simplifying this one to just default the Id value to 0
            //so EF can give the Pokemon Id a number itself
            //if Id is missing from the Body, then Id would be 0 anyway
            //but doing this line will ensure that Id is always 0
            //whether or not the user passed the Id as 0, missing Id or more than 0
            //EF will assign ID/Primary Key number itself if Id = 0
            inputPokemon.Id = 0;

            var pokemon = _mapper.Map<Pokemon>(inputPokemon);

            if (!_pokemonRepository.CreatePokemon(inputPokemon.categoryId, 
                inputPokemon.ownerId, pokemon))
            {
                ModelState.AddModelError("Create Error", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, pokemon);

        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory([FromRoute] int pokeId, [FromBody] PokemonDto inputPokemon)
        {
            if (inputPokemon == null)
                return BadRequest(ModelState);

            if (inputPokemon.Id != pokeId)
            {
                ModelState.AddModelError("Incorrect IDS", "The given IDs do not match from the route and body");
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mappedPokemon = _mapper.Map<Pokemon>(inputPokemon);

            if (!_pokemonRepository.UpdatePokemon(mappedPokemon))
            {
                ModelState.AddModelError("Update Error", "Something went wrong while updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeletePokemon([FromRoute] int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_pokemonRepository.DeletePokemon(pokeId))
            {
                ModelState.AddModelError("Delete Error", "Something went wrong while deleting pokemon");
                return StatusCode(500, ModelState);
            }

            return Ok($"Deleted pokemon {pokeId} successfully");
        }
    }
}
