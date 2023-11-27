using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PocemonReview.DTO;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategorrRepository _repository;
        private readonly IMapper _mapper;
        public CategoryController(ICategorrRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetAllCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_repository.GetAllCategories());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categories);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id)
        {
            if (!_repository.CategoryExist(id))
                return NotFound();

            var category = _mapper.Map<CategoryDTO>(_repository.GetCategory(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }
        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_repository.GetPokemonByCategoryId(categoryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreated)
        {
            if (categoryCreated == null) 
                return BadRequest(ModelState);
            
            var category = _repository.GetAllCategories().Where(c => c.Name.Trim().ToUpper() == categoryCreated.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreated);

            if (!_repository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Soomething went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Category was succesfully created!!!");
        }
        [HttpPut("categoryId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            if (categoryId != updatedCategory.Id)
                return BadRequest(ModelState);

            if (!_repository.CategoryExist(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_repository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updateing");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
