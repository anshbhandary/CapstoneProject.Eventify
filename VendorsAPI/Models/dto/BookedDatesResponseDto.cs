namespace VendorsAPI.Models.dto
{
    public class BookedDatesResponseDto
    {
        public int VendorId { get; set; }
        public List<DateTime> BookedDates { get; set; } = new();
    }
}
