using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorAPI.Models.dto;
using VendorAPI.Repository;

namespace VendorAPI.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public BookingController(IBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto dto)
        {
            try
            {
                var result = await _repo.CreateBooking(dto);
                _response.Result = result;
                _response.Message = "Booking created successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var success = await _repo.CancelBooking(id);
                _response.Result = success;
                _response.Message = success ? "Booking cancelled." : "Booking not found.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetBookingsByCustomer(int customerId)
        {
            try
            {
                var result = await _repo.GetBookingsByCustomerId(customerId);
                _response.Result = result;
                _response.Message = "Bookings retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }
    }
}
