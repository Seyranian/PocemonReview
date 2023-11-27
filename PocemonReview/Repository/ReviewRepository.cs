using PocemonReview.Data;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Repository
{
    public class ReviewRepository : IReviwRepository
    {
        private readonly DataContext _context;
        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();
        }

        public bool ReviewExist(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
