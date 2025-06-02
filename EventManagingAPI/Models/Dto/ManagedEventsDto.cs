using System.ComponentModel.DataAnnotations;

namespace EventManagingAPI.Models.Dto
{
    public class ManagedEventDTO
    {
        public int VendorId { get; set; }
        public int EventRequestId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateOfEvent { get; set; }
    }

    public class ManagedEventResponseDTO
    {
        public int ManagedEventId { get; set; }
        public int VendorId { get; set; }
        public int EventRequestId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateOfEvent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


