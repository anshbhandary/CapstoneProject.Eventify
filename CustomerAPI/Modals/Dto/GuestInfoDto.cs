namespace CustomerAPI.Modals.Dto
{
    public class GuestInfoDto
    {
        public int GuestId { get; set; }
        public int EventRequirementId { get; set; }
        public string GuestName { get; set; }
        public string Email { get; set; }
        public string MealPreference { get; set; }
        public bool IsAttending { get; set; }
    }
}
