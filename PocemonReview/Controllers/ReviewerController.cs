using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocemonReview.DTO;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _repository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<ICollection<ReviewerDTO>>(_repository.GetReviewers());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewers);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int id)
        {
            if (!_repository.ReviewerExist(id))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDTO>(_repository.GetReviewer(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }
        [HttpGet("review/{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int id)
        {
            var reviews = _mapper.Map<List<ReviwDTO>>(_repository.GetReviewsByReviewer(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerCreated)
        {
            if (reviewerCreated == null)
                return BadRequest(ModelState);

            var reviewer = _repository.GetReviewers().Where(r => r.LastName.Trim().ToUpper() == reviewerCreated.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreated);

            if (!_repository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Reviewer was succesfully created!!!"); 
        }
    }
}
