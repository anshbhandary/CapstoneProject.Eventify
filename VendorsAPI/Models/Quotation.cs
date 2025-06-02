using System.ComponentModel.DataAnnotations;

namespace VendorAPI.Models
{
    public class Quotation
    {
        [Key]
        public int QuoteId { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        public decimal? TotalPrice { get; set; }

        public decimal? Discount { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
