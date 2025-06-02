using AdminAPI.Models;
using AdminAPI.Models.Dto;
using AdminAPI.Repository.Interfaces;
using AutoMapper;
using EventManagingAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public AdminController(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpPost]
        public ResponseDto CreateAdmin([FromBody] AdminDto adminDto)
        {
            try
            {
                var user = _mapper.Map<Admin>(adminDto);
                _adminRepository.Add(user);
                _adminRepository.SaveChanges();
                _responseDto.Result = _mapper.Map<AdminDto>(user);
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }
    }
}
