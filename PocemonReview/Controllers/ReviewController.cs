using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocemonReview.DTO;
using PocemonReview.IRepository;
using PocemonReview.Models;
using PocemonReview.Repository;

namespace PocemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviwRepository _repository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        public ReviewController(IReviwRepository repository,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _repository = repository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<ICollection<ReviwDTO>>(_repository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int id)
        {
            if (!_repository.ReviewExist(id))
                return NotFound();

            var review = _mapper.Map<ReviwDTO>(_repository.GetReview(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokemonId)
        {
            var reviews = _mapper.Map<List<ReviwDTO>>(_repository.GetReviewsOfAPokemon(pokemonId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int pokemonId, [FromQuery] int reviewerId, [FromBody] ReviwDTO reviewCreated)
        {
            if (reviewCreated == null)
                return BadRequest(ModelState);

            var review = _repository.GetReviews().Where(r => r.Title.Trim().ToUpper() == reviewCreated.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreated);
            reviewMap.Pokemon = _pokemonRepository.GetPokemonById(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_repository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something wnet wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Review was succesfully created!!!");
        }
    }
}
