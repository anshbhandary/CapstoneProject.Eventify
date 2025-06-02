using System.ComponentModel.DataAnnotations;

namespace CustomerAPI.Modals
{
    public class EventRequirements
    {
        [Key]
        [Required]
        public int EventRequirementId { get; set; }

        [Required]              
        public string CustomerId { get; set; }
        public string EventType { get; set; }
        public string Location { get; set; }
        public DateTime EventDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string SpecialRequests { get; set; }
        public int Budget { get; set; } 
        public string Description {  get; set; }
    }
}
