using Microsoft.EntityFrameworkCore;
using VendorAPI.Data;
using VendorAPI.Models;
using VendorAPI.Models.dto;
using VendorAPI.Repository;
using VendorsAPI.Models.dto;

public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly AppDbContext _context;

    public AvailabilityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookedDatesResponseDto> GetBookedDates(int vendorId)
    {
        var bookedSlots = await _context.AvailabilitySlots
            .Where(slot => slot.VendorId == vendorId && slot.IsBooked)
            .ToListAsync();

        // Collect all individual dates
        var bookedDates = bookedSlots
            .Select(slot => slot.BookedDate.Date)
            .ToList();

        return new BookedDatesResponseDto
        {
            VendorId = vendorId,
            BookedDates = bookedDates
        };
    }

    public async Task<AvailabilitySlotDto> AddOrUpdateSlot(AvailabilitySlotDto dto)
    {
        var existingSlot = await _context.AvailabilitySlots
            .FirstOrDefaultAsync(s => s.VendorId == dto.VendorId && s.BookedDate.Date == dto.BookedDate.Date);

        if (existingSlot != null)
        {
            existingSlot.IsBooked = dto.IsBooked;
        }
        else
        {
            var newSlot = new AvailabilitySlot
            {
                VendorId = dto.VendorId,
                BookedDate = dto.BookedDate.Date,
                IsBooked = dto.IsBooked
            };
            _context.AvailabilitySlots.Add(newSlot);
        }

        await _context.SaveChangesAsync();
        return dto;
    }
}
