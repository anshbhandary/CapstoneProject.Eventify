using AutoMapper;
using VendorAPI.Data;
using VendorAPI.Models.dto;
using VendorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VendorAPI.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Booking> CreateBooking(BookingDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return false;
            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerId(int customerId)
        {
            return await _context.Bookings.Where(b => b.CustomerId == customerId).ToListAsync();
        }
    }
}
