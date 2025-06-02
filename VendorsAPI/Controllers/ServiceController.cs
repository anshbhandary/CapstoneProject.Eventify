using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorAPI.Models.dto;
using VendorAPI.Repository;

namespace VendorAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _repo;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public ServiceController(IServiceRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpPost("select")]
        public async Task<IActionResult> SelectService([FromBody] ServiceSelectionDto dto)
        {
            try
            {

                var result = await _repo.SelectService(dto);
                _response.Result = result;
                _response.IsSuccess = true;
                _response.Message = "Service selected successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateService([FromBody] ServiceSelectionUpdateDto dto)
        {
            try
            {
                var result = await _repo.UpdateService(dto);
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Service not found.";
                    return NotFound(_response);
                }

                _response.Result = result;
                _response.Message = "Service updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var result = await _repo.DeleteService(id);
                if (!result)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Service not found or already deleted.";
                    return NotFound(_response);
                }

                _response.Message = "Service deleted successfully.";
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
