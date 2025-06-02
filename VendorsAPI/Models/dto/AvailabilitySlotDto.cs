namespace VendorAPI.Models.dto
{
    public class AvailabilitySlotDto
    {

        public int VendorId { get; set; }

        // The date on which availability is being set
        public DateTime BookedDate { get; set; }

        // Whether the vendor is booked (true) or available (false) on that day
        public bool IsBooked { get; set; }
    }
}
