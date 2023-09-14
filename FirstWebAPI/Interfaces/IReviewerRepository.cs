using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface IReviewerRepository
    {
        //GET READ
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool ReviewerExists(int reviewerId);
        bool ReviewerExists(string firstName, string lastName);

        //POST CREATE
        bool CreateReviewer(Reviewer reviewer);
        //PUT UPDATE
        bool UpdateReviewer(Reviewer reviewer);

        //DELETE
        bool DeleteReviewer(int reviewerId);
        bool Save();
    }
}
