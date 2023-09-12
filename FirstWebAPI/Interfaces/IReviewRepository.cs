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
        bool Save();
    }
}
