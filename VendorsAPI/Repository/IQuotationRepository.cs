using VendorAPI.Models.dto;
using VendorAPI.Models;

namespace VendorAPI.Repository
{
    public interface IQuotationRepository
    {
        Task<Quotation> SendQuotation(QuotationDto dto);
        Task<Quotation> ApplyQuotation(int id);
        Task<IEnumerable<Quotation>> GetAllQuotations();
        Task<Quotation> GetQuotationById(int id);
    }
}
