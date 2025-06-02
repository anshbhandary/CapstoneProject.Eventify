using System.ComponentModel.DataAnnotations;

namespace EventManagingAPI.Models
{
    public class ManagedEvent
    {
        [Key]
        public int ManagedEventId { get; set; }
        [Required]
        public int VendorId { get; set; }
        [Required]
        public int EventRequestId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public DateTime DateOfEvent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<ToDoItem> ToDoItems { get; set; }
        public ICollection<ItemRequired> ItemRequirements { get; set; }
    }
}
