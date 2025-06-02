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
    public class ApprovalController : ControllerBase
    {
        private readonly IAprovalRepository _approvalRepository;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public ApprovalController(IAprovalRepository approvalRepository, IMapper mapper)
        {
            _approvalRepository = approvalRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet("{id:int}")]
        public ResponseDto GetById(int id)
        {
            try
            {
                var obj = _approvalRepository.GetStatus(id);
                _responseDto.Result = _mapper.Map<ApprovalResponseDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                var objList = _approvalRepository.GetAll();
                _responseDto.Result = _mapper.Map<IEnumerable<ApprovalResponseDto>>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto CreateRequest([FromBody] ApprovalDto approvalDto)
        {
            try
            {
                var obj = _mapper.Map<Approval>(approvalDto);

                _approvalRepository.Add(obj);
                _approvalRepository.SaveChanges();

                _responseDto.Result = _mapper.Map<ApprovalResponseDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        public ResponseDto UpdateStatus([FromBody] ApprovalUpdateDto approvalUpdateDto)
        {
            try
            {
                var existingApproval = _approvalRepository.GetStatus(approvalUpdateDto.ApprovalRequestId);
                if (existingApproval == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Approval request not found.";
                }

                // Update only the fields allowed
                existingApproval.ApprovalStatus = approvalUpdateDto.ApprovalStatus;
                existingApproval.VendorId = approvalUpdateDto.VendorId;

                _approvalRepository.Update(existingApproval);
                _approvalRepository.SaveChanges();

                _responseDto.Result = _mapper.Map<ApprovalResponseDto>(existingApproval);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

    }
}
