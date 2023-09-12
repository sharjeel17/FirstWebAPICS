namespace FirstWebAPI.Dto
{
    public class CreateReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int Rating { get; set; }
        public int reviewerId { get; set; }
        public int pokemonId { get; set; }
    }
}
