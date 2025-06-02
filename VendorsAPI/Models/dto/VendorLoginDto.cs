using System.ComponentModel.DataAnnotations;

namespace VendorAPI.Models.dto
{
    public class VendorLoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
