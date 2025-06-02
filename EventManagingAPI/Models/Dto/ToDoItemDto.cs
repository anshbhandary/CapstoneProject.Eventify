using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventManagingAPI.Models.Dto
{
    public class ToDoItemDTO
    {
        public int ManagedEventId { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class ToDoItemResponseDTO
    {
        public int ToDoItemId { get; set; }
        public int ManagedEventId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ToDoItemUpdateDTO
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
