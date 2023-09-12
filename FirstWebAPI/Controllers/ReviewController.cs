using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Interfaces;
using FirstWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokeRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,
            IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository ,IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokeRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<ICollection<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId)) return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsByPokemon(int pokeId)
        {
            if (!_pokeRepository.PokemonExists(pokeId)) return NotFound();

            var reviews = _mapper.Map<ICollection<ReviewDto>>(_reviewRepository.GetReviewsOfPokemon(pokeId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview(CreateReviewDto inputReview) 
        {
            if (inputReview == null)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(inputReview.reviewerId))
                ModelState.AddModelError("Reviewer ID Error", "Entered Reviewer ID does not exist");

            if (!_pokeRepository.PokemonExists(inputReview.pokemonId))
                ModelState.AddModelError("Pokemon ID Error", "Entered Pokemon ID does not exist");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            inputReview.Id = 0;

            var mappedReview = _mapper.Map<Review>(inputReview);

            //
            mappedReview.Pokemon = _pokeRepository.GetPokemon(inputReview.pokemonId);
            mappedReview.Reviewer = _reviewerRepository.GetReviewer(inputReview.reviewerId);

            if (!_reviewRepository.CreateReview(mappedReview)) 
            {
                ModelState.AddModelError("Creation Error", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, mappedReview);
        }
    }
}
