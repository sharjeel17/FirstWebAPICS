using FirstWebAPI.Models;

namespace FirstWebAPI.Interfaces
{
    public interface IReviewRepository
    {
        //GET READ
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfPokemon(int pokeId);
        bool ReviewExists(int reviewId);

        //POST CREATE
        bool CreateReview(Review review);

        //PUT UPDATE
        bool UpdateReview(Review review); 

        //DELETE
        bool DeleteReview(int reviewId);
        bool Save();
    }
}
