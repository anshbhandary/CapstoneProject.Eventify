using AutoMapper;
using VendorAPI.Data;
using VendorAPI.Models.dto;
using VendorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VendorAPI.Repository
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public QuotationRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Quotation> SendQuotation(QuotationDto dto)
        {
            var quote = _mapper.Map<Quotation>(dto);
            _context.Quotations.Add(quote);
            await _context.SaveChangesAsync();
            return quote;
        }

        public async Task<Quotation> ApplyQuotation(int id)
        {
            var quote = await _context.Quotations.FindAsync(id);
            if (quote == null) return null;
            // Additional apply logic could be added here
            return quote;
        }

        public async Task<IEnumerable<Quotation>> GetAllQuotations()
        {
            return await _context.Quotations.ToListAsync();
        }

        public async Task<Quotation> GetQuotationById(int id)
        {
            return await _context.Quotations.FindAsync(id);
        }
    
}
}
