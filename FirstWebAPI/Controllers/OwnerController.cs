using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository,
            IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners() 
        {
            var owners = _mapper.Map<ICollection<OwnerDto>>(_ownerRepository.GetOwners());

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwner(int ownerId) 
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonsByOwner(int ownerId) 
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

            var pokemons = _mapper.Map<ICollection<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(ownerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateOwner([FromBody] OwnerDto owner, [FromQuery] int countryId) 
        {
            if (owner == null) return BadRequest(ModelState);

            if (_ownerRepository.OwnerExists(owner.Id))
                ModelState.AddModelError("ID Error", "Owner ID already exists");

            if (_ownerRepository.OwnerExists(owner.FirstName, owner.LastName))
                ModelState.AddModelError("Name Error", "Owner name already exists");

            if (!_countryRepository.CountryExists(countryId))
                ModelState.AddModelError("Country ID Error", "Please enter a valid country ID in query");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            owner.Id = 0;
            Owner mappedOwner = _mapper.Map<Owner>(owner);

            //Add country, with countryId from query string, to the Owner
            //as it is a Country to Owner - One to Many relationship
            //and each Owner needs a Country as the Country is a Foreign Key inside of Owner
            mappedOwner.Country = _countryRepository.GetCountry(countryId);

            if(!_ownerRepository.CreateOwner(mappedOwner))
            {
                ModelState.AddModelError("Create Error", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, mappedOwner);
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory([FromRoute] int ownerId, [FromBody] OwnerDto inputOwner)
        {
            if (inputOwner == null)
                return BadRequest(ModelState);

            if (inputOwner.Id != ownerId)
            {
                ModelState.AddModelError("Incorrect IDS", "The given IDs do not match from the route and body");
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mappedOwner = _mapper.Map<Owner>(inputOwner);

            if (!_ownerRepository.UpdateOwner(mappedOwner))
            {
                ModelState.AddModelError("Update Error", "Something went wrong while updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
