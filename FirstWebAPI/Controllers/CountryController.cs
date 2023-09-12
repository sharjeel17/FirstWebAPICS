using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
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
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<ICollection<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCountry(int countryId)
        {
            if (!(_countryRepository.CountryExists(countryId))) return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCountryOfOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry([FromBody] CountryDto country) 
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
            if (!_countryRepository.CreateCountry(mappedCountry)) 
            {
                ModelState.AddModelError("Create Error", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, mappedCountry);

        }
    }
}
