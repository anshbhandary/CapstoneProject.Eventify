        using System.ComponentModel.DataAnnotations;

namespace CustomerAPI.Modals
{
    public class ServiceRequest
    {
        [Key]
        public int RequestId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public int EventRequirementId { get; set; }

        public DateTime RequestedAt { get; set; }
        public string Status { get; set; } = "Pending"; // Pending / Accepted / Rejected
    }
}
