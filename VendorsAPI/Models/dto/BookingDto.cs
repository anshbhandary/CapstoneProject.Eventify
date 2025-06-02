namespace VendorAPI.Models.dto
{
    public class BookingDto
    {
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public int PackageId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
