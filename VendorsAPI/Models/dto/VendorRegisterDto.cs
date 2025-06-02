using System.ComponentModel.DataAnnotations;

namespace VendorAPI.Models.dto
{
    public class VendorRegisterDto
    {
        [Required]
        public string BusinessName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
