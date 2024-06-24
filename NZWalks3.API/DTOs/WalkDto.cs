namespace NZWalks3.API.DTOs
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public RequestRegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }

    }
}
