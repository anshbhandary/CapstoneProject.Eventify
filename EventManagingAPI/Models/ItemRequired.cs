using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventManagingAPI.Models
{
    public class ItemRequired
    {
        [Key]
        public int RequiredItemId { get; set; }
        [Required]
        public int ManagedEventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } = 1;
        public bool IsAcquired { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public ManagedEvent ManagedEvent { get; set; }
    }
}