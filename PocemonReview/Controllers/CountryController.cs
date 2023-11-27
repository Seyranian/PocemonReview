using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PocemonReview.DTO;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetAllCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>(_repository.GetAllCountries());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int id)
        {
            if (!_repository.CountryExist(id))
                return NotFound();

            var country = _mapper.Map<CountryDTO>(_repository.GetCountry(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }
        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwnerId(int ownerId)
        {
            var country = _mapper.Map<CountryDTO>(_repository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO countryCreated)
        {
            if(countryCreated == null)
                return BadRequest(ModelState);

            var country = _repository.GetAllCountries().Where(c => c.Name.Trim().ToUpper() == countryCreated.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreated);

            if (!_repository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something wnet wrong while saving");
                return StatusCode(500,ModelState);
            }
            return Ok("Country was succesfully created!!!");
        }
        [HttpPut("countryId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDTO updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_repository.CountryExist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_repository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updateing");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
