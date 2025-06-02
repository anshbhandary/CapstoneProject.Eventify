using VendorAPI.Models.dto;
using VendorAPI.Models;

namespace VendorAPI.Repository
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(BookingDto dto);
        Task<bool> CancelBooking(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByCustomerId(int customerId);
    }
}
