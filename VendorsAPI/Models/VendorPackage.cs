using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorAPI.Models
{
    public class VendorPackage
    {
        public int Id { get; set; }

        [Required]
        public string PackageName { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }
    }
}
