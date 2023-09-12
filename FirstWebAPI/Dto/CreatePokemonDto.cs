namespace FirstWebAPI.Dto
{
    public class CreatePokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int categoryId { get; set; }
        public int ownerId { get; set; }
    }
}
