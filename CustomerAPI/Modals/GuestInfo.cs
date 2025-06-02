using System.ComponentModel.DataAnnotations;

namespace CustomerAPI.Modals
{
    public class GuestInfo
    {
        [Key]
        public int GuestId { get; set; }

        [Required]
        public int RequestId { get; set; }

        public string GuestName { get; set; }
        public string MealPreference { get; set; }
        public bool IsAttending { get; set; }
    }
}
