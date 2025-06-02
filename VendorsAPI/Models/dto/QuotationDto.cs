namespace VendorAPI.Models.dto
{
    public class QuotationDto
    {
        public int VendorId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
