using PocemonReview.Data;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Repository
{
    public class CategoryRepository : ICategorrRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExist(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategoryId(int categoryId)
        {
            return _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save(); 
        }
    }
}
