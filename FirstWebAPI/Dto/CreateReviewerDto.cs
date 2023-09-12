namespace FirstWebAPI.Dto
{
    public class CreateReviewerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
