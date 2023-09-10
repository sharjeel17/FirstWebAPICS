using FirstWebAPI.Models;

namespace FirstWebAPI.Dto
{
    public class ReviewerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = null!;
    }
}
