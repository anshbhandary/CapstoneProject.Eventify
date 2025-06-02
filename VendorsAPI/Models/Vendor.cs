using System.ComponentModel.DataAnnotations;

namespace VendorAPI.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        [Required]
        public string BusinessName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<VendorPackage> Packages { get; set; }


    }
}
