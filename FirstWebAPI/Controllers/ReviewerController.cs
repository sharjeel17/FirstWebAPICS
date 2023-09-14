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
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper) 
        {
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers() 
        {
            var reviewers = _mapper.Map<ICollection<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewer(int reviewerId) 
        {
            if(!(_reviewerRepository.ReviewerExists(reviewerId))) return NotFound();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsByReviewer(int reviewerId) 
        {
            if (!(_reviewerRepository.ReviewerExists(reviewerId))) return NotFound();

            var reviews = _mapper.Map<ICollection<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateReviewer([FromBody] CreateReviewerDto inputReviewer) 
        {
            if (inputReviewer == null)
                return BadRequest(ModelState);

            if (_reviewerRepository.ReviewerExists(inputReviewer.FirstName, inputReviewer.LastName))
                ModelState.AddModelError("Reviewer Exists", "Entered reviewer already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mappedReviewer = _mapper.Map<Reviewer>(inputReviewer);
            mappedReviewer.Id = 0;

            if (!_reviewerRepository.CreateReviewer(mappedReviewer)) 
            {
                ModelState.AddModelError("Creation Error", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201,mappedReviewer);
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReview([FromRoute] int reviewerId, [FromBody] CreateReviewerDto inputReviewer)
        {
            if (inputReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != inputReviewer.Id)
            {
                ModelState.AddModelError("Incorrect IDS", "The given IDs do not match from the route and body");
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mappedReviewer = _mapper.Map<Reviewer>(inputReviewer);

            if (!_reviewerRepository.UpdateReviewer(mappedReviewer))
            {
                ModelState.AddModelError("Update Error", "Something went wrong while updating reviewer");
                return StatusCode(500, ModelState);
            }

            return StatusCode(200, mappedReviewer);
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewer([FromRoute] int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerId))
            {
                ModelState.AddModelError("Delete Error", "Something went wrong while deleting reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok($"Deleted reviewer {reviewerId} successfully");
        }
    }
}
