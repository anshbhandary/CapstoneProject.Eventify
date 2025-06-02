using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventManagingAPI.Models
{
    public class ToDoItem
    {
        [Key]
        public int ToDoItemId { get; set; }
        [Required]
        public int ManagedEventId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        // Navigation property
        public ManagedEvent ManagedEvent { get; set; }
    }
}