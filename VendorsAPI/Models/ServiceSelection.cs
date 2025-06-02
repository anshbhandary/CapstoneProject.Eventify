using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendorAPI.Models
{
    public class ServiceSelection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        public int PackageId { get; set; }

        public DateTime SelectedOn { get; set; } = DateTime.UtcNow;

    }
}
