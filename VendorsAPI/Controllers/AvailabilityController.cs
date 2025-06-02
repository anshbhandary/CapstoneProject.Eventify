using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorAPI.Models.dto;
using VendorAPI.Repository;

namespace VendorAPI.Controllers
{
    [Route("api/availability")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityRepository _repo;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public AvailabilityController(IAvailabilityRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetBookedDatesByVendor(int vendorId)
        {
            try
            {
                var booked = await _repo.GetBookedDates(vendorId);

                if (booked == null || booked.BookedDates == null || !booked.BookedDates.Any())
                {
                    _response.IsSuccess = true;
                    _response.Message = "No bookings found.";
                    _response.Result = new List<string>();
                    return Ok(_response);
                }

                // Format each date if you want to return strings
                var formattedDates = booked.BookedDates
                    .Select(d => d.ToString("yyyy-MM-dd"))
                    .ToList();

                _response.Result = new
                {
                    VendorId = booked.VendorId,
                    BookedDates = formattedDates
                };
                _response.IsSuccess = true;
                _response.Message = "Booked dates retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> AddOrUpdateSlot([FromBody] AvailabilitySlotDto dto)
        {
            try
            {
                var result = await _repo.AddOrUpdateSlot(dto);
                _response.Result = result;
                _response.IsSuccess = true;
                _response.Message = "Slot updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error: {ex.Message}";
                return StatusCode(500, _response);
            }
        }
    }
}
