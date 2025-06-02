using System.ComponentModel.DataAnnotations;

namespace CustomerAPI.Modals.Dto
{
    public class CustomerDto
    {
        [Key]
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
