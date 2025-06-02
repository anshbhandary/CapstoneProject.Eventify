using System.ComponentModel.DataAnnotations;

namespace AdminAPI.Models
{
    public class Approval
    {
        [Key]
        public int ApprovalRequestId { get; set; }
        [Required]
        public int VendorId { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public string DocumentsUrl { get; set; }
        [Required]
        public bool ApprovalStatus { get; set; } = false;

        public int? AdminId { get; set; }
    }
}
