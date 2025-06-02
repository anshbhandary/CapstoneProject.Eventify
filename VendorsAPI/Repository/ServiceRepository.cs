using AutoMapper;
using VendorAPI.Data;
using VendorAPI.Models.dto;
using VendorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VendorAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ServiceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceSelection> SelectService(ServiceSelectionDto dto)
        {
            var selection = _mapper.Map<ServiceSelection>(dto);
            selection.SelectedOn = DateTime.UtcNow;
            _context.ServiceSelections.Add(selection);
            await _context.SaveChangesAsync();
            return selection;
        }

        public async Task<ServiceSelection> UpdateService(ServiceSelectionUpdateDto dto)
        {
            var selection = await _context.ServiceSelections.FindAsync(dto.Id);
            if (selection == null) return null;
            _mapper.Map(dto, selection);
            await _context.SaveChangesAsync();
            return selection;
        }

        public async Task<bool> DeleteService(int id)
        {
            var selection = await _context.ServiceSelections.FindAsync(id);
            if (selection == null) return false;
            _context.ServiceSelections.Remove(selection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ServiceSelection> GetServiceById(int id)
        {
            return await _context.ServiceSelections.FindAsync(id);
        }

        public async Task<IEnumerable<ServiceSelection>> GetAllServices()
        {
            return await _context.ServiceSelections.ToListAsync();
        }
    }
}
