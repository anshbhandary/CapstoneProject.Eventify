namespace GlobalAPI.Models.Dto
{
    public class GeoSearchRequestDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
