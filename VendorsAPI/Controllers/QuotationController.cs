using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VendorAPI.Models.dto;
using VendorAPI.Repository;

namespace VendorAPI.Controllers
{
    [Route("api/quotations")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationRepository _repo;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public QuotationController(IQuotationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendQuotation([FromBody] QuotationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid quotation data.";
                    return BadRequest(_response);
                }

                var result = await _repo.SendQuotation(dto);
                _response.Result = result;
                _response.IsSuccess = true;
                _response.Message = "Quotation sent successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPost("apply/{id}")]
        public async Task<IActionResult> ApplyQuotation(int id)
        {
            try
            {
                var appliedQuotation = await _repo.ApplyQuotation(id);
                if (appliedQuotation == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Quotation not found.";
                    return NotFound(_response);
                }

                _response.Result = appliedQuotation;
                _response.IsSuccess = true;
                _response.Message = "Quotation applied successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuotations()
        {
            try
            {
                var quotations = await _repo.GetAllQuotations();
                _response.Result = quotations;
                _response.IsSuccess = true;
                _response.Message = "Quotations retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuotationById(int id)
        {
            try
            {
                var quotation = await _repo.GetQuotationById(id);
                if (quotation == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Quotation not found.";
                    return NotFound(_response);
                }

                _response.Result = quotation;
                _response.IsSuccess = true;
                _response.Message = "Quotation retrieved successfully.";
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
