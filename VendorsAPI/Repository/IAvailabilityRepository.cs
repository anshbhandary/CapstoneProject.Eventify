using VendorAPI.Models.dto;
using VendorAPI.Models;
using VendorsAPI.Models.dto;

namespace VendorAPI.Repository
{
    public interface IAvailabilityRepository
    {
        Task<BookedDatesResponseDto> GetBookedDates(int vendorId);
        Task<AvailabilitySlotDto> AddOrUpdateSlot(AvailabilitySlotDto dto);
    }
}
