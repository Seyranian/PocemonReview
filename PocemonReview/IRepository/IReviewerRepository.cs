using PocemonReview.Models;

namespace PocemonReview.IRepository
{
    public interface IReviewerRepository
    {
       ICollection<Reviewer> GetReviewers();
       Reviewer GetReviewer(int id);
       ICollection<Review> GetReviewsByReviewer(int id);
       bool ReviewerExist(int id);
       bool CreateReviewer(Reviewer reviewer);
       bool Save();
    }
}
