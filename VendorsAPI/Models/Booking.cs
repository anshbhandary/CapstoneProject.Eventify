using System.ComponentModel.DataAnnotations;

namespace VendorAPI.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        public string Status { get; set; } = "Pending";
    
}
}
