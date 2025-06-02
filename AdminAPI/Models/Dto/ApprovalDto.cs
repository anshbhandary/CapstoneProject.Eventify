namespace AdminAPI.Models.Dto
{
    public class ApprovalDto
    {
        public int VendorId { get; set; }
        public string Details { get; set; }
        public string DocumentsUrl { get; set; }
        public bool ApprovalStatus { get; set; } = false;
    }

    public class ApprovalResponseDto
    {
        public int ApprovalRequestId { get; set; }
        public int VendorId { get; set; }
        public string Details { get; set; }
        public string DocumentsUrl { get; set; }
        public bool ApprovalStatus { get; set; } = false;
    }

    public class ApprovalUpdateDto
    {
        public int ApprovalRequestId { get; set; }
        public int VendorId { get; set; }
        public bool ApprovalStatus { get; set; }
    }
}