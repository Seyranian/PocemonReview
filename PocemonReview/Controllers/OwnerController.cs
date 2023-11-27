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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _repository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository repository,ICountryRepository countryRepository, IMapper mapper)
        {
            _repository = repository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetAllOwners()
        {
            var owners = _mapper.Map<List<OwnerDTO>>(_repository.GetAllOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int id)
        {
            if (!_repository.OwnerExist(id))
                return NotFound();

            var owner = _mapper.Map<OwnerDTO>(_repository.GetOwner(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }
        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_repository.OwnerExist(ownerId))
                return NotFound();
            
            var pokemons = _mapper.Map<List<PokemonDTO>>(_repository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] OwnerDTO ownerCreated, [FromQuery] int countryId)
        {
            if (ownerCreated == null)
                return BadRequest(ModelState);

            var country = _repository.GetAllOwners().Where(c => c.FirstName.Trim().ToUpper() == ownerCreated.FirstName.TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreated);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_repository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something wnet wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Owner was succesfully created!!!");
        }
        [HttpPut("ownerId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCategory(int ownerId, [FromBody] OwnerDTO updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);

            if (!_repository.OwnerExist(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            if (!_repository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updateing");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
