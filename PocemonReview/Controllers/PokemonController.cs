using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PocemonReview.DTO;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _repository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository,IMapper mapper)
        {
            _repository = pokemonRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type =  typeof(IEnumerable<Pokemon>))]
        public IActionResult GetAllPokemons()
        {
            var pokemons = _mapper.Map<ICollection<PokemonDTO>>(_repository.GetAllPokemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonById(int id)
        {
            if (!_repository.PokemonExists(id))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDTO>(_repository.GetPokemonById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }
        [HttpGet("id/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_repository.PokemonExists(id))
                return NotFound();

            var pokemonRating = _repository.GetPokemonRating(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonRating);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId,[FromBody] PokemonDTO pokemonCreated)
        {
            if (pokemonCreated == null)
                return BadRequest(ModelState);

            var pokemon = _repository.GetAllPokemons().Where(p => p.Name.Trim().ToUpper() == pokemonCreated.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreated);

            if (!_repository.CreatePokemon(ownerId,categoryId,pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saveing");
                return StatusCode(500,ModelState);
            }
            return Ok("Pokemon was succesfully cretaed!!!");
        }
    }
}
