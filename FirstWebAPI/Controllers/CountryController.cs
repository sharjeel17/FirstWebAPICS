using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using FirstWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountries()
        {
            var countries = _mapper.Map<ICollection<CountryDto>>(await _countryRepository.GetCountriesAsync());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            if (!(_countryRepository.CountryExists(countryId))) return NotFound();

            var country = _mapper.Map<CountryDto>(await _countryRepository.GetCountryAsync(countryId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountryOfOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(await _countryRepository.GetCountryByOwnerAsync(ownerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDto country) 
        {
            //check comments on CategorController to see what each is doing
            if (country == null) return BadRequest(ModelState);

            if (_countryRepository.CountryExists(country.Id))
                ModelState.AddModelError("ID Error", "Country with that ID already exists");

            if (_countryRepository.CountryExists(country.Name))
                ModelState.AddModelError("Name Error", "Country with that name already exists");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            country.Id = 0;

            Country mappedCountry = _mapper.Map<Country>(country);
            if (!await _countryRepository.CreateCountryAsync(mappedCountry)) 
            {
                ModelState.AddModelError("Create Error", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, mappedCountry);

        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCountry([FromRoute] int countryId, [FromBody] CountryDto inputCountry)
        {
            if (inputCountry == null)
                return BadRequest(ModelState);

            if (inputCountry.Id != countryId)
            {
                ModelState.AddModelError("Incorrect IDS", "The given IDs do not match from the route and body");
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mappedCountry = _mapper.Map<Country>(inputCountry);

            if (!await _countryRepository.UpdateCountryAsync(mappedCountry))
            {
                ModelState.AddModelError("Update Error", "Something went wrong while updating country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCountry([FromRoute] int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _countryRepository.DeleteCountryAsync(countryId))
            {
                ModelState.AddModelError("Delete Error", "Something went wrong while deleting country");
                return StatusCode(500, ModelState);
            }

            return Ok($"Deleted country {countryId} successfully");
        }
    }
}
