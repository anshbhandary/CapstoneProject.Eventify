using System.ComponentModel.DataAnnotations;

namespace VendorAPI.Models
{
    public class AvailabilitySlot
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public DateTime BookedDate { get; set; }
        public bool IsBooked { get; set; }
    }
}
