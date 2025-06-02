using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventManagingAPI.Models.Dto
{
    public class RequiredItemDTO
    {
        public int ManagedEventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class RequiredItemResponseDTO
    {
        public int RequiredItemId { get; set; }
        public int ManagedEventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool IsAcquired { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RequiredItemUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool IsAcquired { get; set; }
    }
}
