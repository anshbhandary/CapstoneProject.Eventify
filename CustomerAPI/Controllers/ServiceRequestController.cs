using AutoMapper;
using CustomerAPI.Data;
using CustomerAPI.Modals.Dto;
using CustomerAPI.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public ServiceRequestController(CustomerDbContext customerDbContext, IMapper mapper)
        {
            _customerDbContext = customerDbContext;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetServiceRequests()
        {
            try
            {
                IEnumerable<ServiceRequest> objList = _customerDbContext.ServiceRequests.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<ServiceRequest>>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id}")]
        public ResponseDto GetServiceRequest(string id)
        {
            try
            {
                List<ServiceRequest> objList = _customerDbContext.ServiceRequests
                    .Where(u => u.CustomerId == id)
                    .ToList();
                _responseDto.Result = _mapper.Map<List<ServiceRequest>>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto AddServiceRequest([FromBody] ServiceRequestDto serviceRequestDto)
        {
            try
            {
                ServiceRequest obj = _mapper.Map<ServiceRequest>(serviceRequestDto);
                _customerDbContext.ServiceRequests.Add(obj);
                _customerDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<ServiceRequest>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        public ResponseDto UpdateServiceDetails([FromBody] ServiceRequestDto serviceRequestDto)
        {
            try
            {
                ServiceRequest obj = _mapper.Map<ServiceRequest>(serviceRequestDto);
                _customerDbContext.ServiceRequests.Update(obj);
                _customerDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<ServiceRequest>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto DeleteServiceRequest(int id)
        {
            try
            {
                ServiceRequest objList = _customerDbContext.ServiceRequests.First(u => u.RequestId == id);
                _customerDbContext.Remove(objList);
                _customerDbContext.SaveChanges();
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
