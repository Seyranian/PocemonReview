using PocemonReview.Models;

namespace PocemonReview.IRepository
{
    public interface IReviwRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
        bool ReviewExist(int id);
        bool CreateReview(Review review);
        bool Save();
    }
}
