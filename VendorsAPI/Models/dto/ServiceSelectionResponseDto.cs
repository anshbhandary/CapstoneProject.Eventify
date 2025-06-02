namespace VendorsAPI.Models.dto
{
    public class ServiceSelectionResponseDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Notes { get; set; }
        public DateTime SelectedOn { get; set; }
    }
}
