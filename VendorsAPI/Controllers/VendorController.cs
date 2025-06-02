using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorAPI.Models.dto;
using VendorAPI.Models;
using VendorAPI.Repository;
using AutoMapper;

namespace VendorAPI.Controllers
{
    [Route("api/vendor")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository _repo;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public VendorController(IVendorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }


        [HttpPost("packages")]
        public async Task<IActionResult> CreatePackage([FromQuery] int vendorId, [FromBody] PackageCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Invalid package data.";
                    return BadRequest(_responseDto);
                }

                var vendor = await _repo.GetVendorById(vendorId);
                if (vendor == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"Vendor with ID {vendorId} not found.";
                    return NotFound(_responseDto);
                }

                var package = _mapper.Map<VendorPackage>(dto);
                package.VendorId = vendorId;

                await _repo.AddPackage(package);

                _responseDto.Result = _mapper.Map<PackageResponseDto>(package);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Package created successfully.";

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error: {ex.Message}";
                return StatusCode(500, _responseDto);
            }
        }


        [HttpGet("packages")]
        public async Task<IActionResult> GetMyPackages([FromQuery] int vendorId)
        {
            try
            {
                var packages = await _repo.GetPackagesByVendorId(vendorId);
                var packageDtos = _mapper.Map<List<PackageResponseDto>>(packages);
                _responseDto.Result = packageDtos;
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Packages retrieved successfully.";
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error: {ex.Message}";
                return StatusCode(500, _responseDto);
            }
           
            
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetVendorDetails([FromQuery] int vendorId)
        {
            try
            {
                var vendor = await _repo.GetVendorById(vendorId);
                if (vendor == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Vendor not found.";
                    return NotFound(_responseDto);
                }

                var vendorDto = _mapper.Map<VendorResponseDto>(vendor);

                _responseDto.Result = vendorDto;
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Vendor details fetched.";

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error: {ex.Message}";
                return StatusCode(500, _responseDto);
            }
            
          
        }

        [HttpDelete("packages/{packageId}")]
        public async Task<IActionResult> DeletePackage(int packageId, [FromQuery] int vendorId)
        {
            try
            {
                var result = await _repo.DeletePackage(packageId, vendorId);
                if (!result)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Package not found or unauthorized.";
                    return NotFound(_responseDto);
                }

                _responseDto.IsSuccess = true;
                _responseDto.Message = "Package deleted successfully.";
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error: {ex.Message}";
                return StatusCode(500, _responseDto);
            }
        }

        [HttpPut("packages/{packageId}")]
        public async Task<IActionResult> UpdatePackage(int packageId, [FromQuery] int vendorId, [FromBody] PackageUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Invalid update data.";
                    return BadRequest(_responseDto);
                }

                var result = await _repo.UpdatePackage(packageId, vendorId, dto);
                if (!result)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Package not found or unauthorized.";
                    return NotFound(_responseDto);
                }

                _responseDto.IsSuccess = true;
                _responseDto.Message = "Package updated successfully.";
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error: {ex.Message}";
                return StatusCode(500, _responseDto);
            }
        }





    }
}
