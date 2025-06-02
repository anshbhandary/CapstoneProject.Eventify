using AutoMapper;
using CustomerAPI.Data;
using CustomerAPI.Modals;
using CustomerAPI.Modals.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public EventsController(CustomerDbContext customerDbContext, IMapper mapper)
        {
            _customerDbContext = customerDbContext;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetEvents()
        {
            try
            {
                IEnumerable<EventRequirements> objList = _customerDbContext.EventRequirements.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<EventRequirementDto>>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("customer/{id}")]
        public ResponseDto GetEventByCustomer(string id)
        {
            try
            {
                List<EventRequirements> objList = _customerDbContext.EventRequirements
                    .Where(u => u.CustomerId == id)
                    .ToList();
                _responseDto.Result = _mapper.Map<List<EventRequirementDto>>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetEvent(int id)
        {
            try
            {
                EventRequirements objList = _customerDbContext.EventRequirements.First(u => u.EventRequirementId == id);
                _responseDto.Result = _mapper.Map<EventRequirementDto>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto AddEventRequirements([FromBody] EventRequirementDto eventRequirementDto)
        {
            try
            {
                EventRequirements obj = _mapper.Map<EventRequirements>(eventRequirementDto);
                _customerDbContext.EventRequirements.Add(obj);
                _customerDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<EventRequirementDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        public ResponseDto UpdateEventDetails([FromBody] EventRequirementDto eventRequirementDto)
        {
            try
            {
                EventRequirements obj = _mapper.Map<EventRequirements>(eventRequirementDto);
                _customerDbContext.EventRequirements.Update(obj);
                _customerDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<EventRequirementDto>(obj);
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
        public ResponseDto DeleteEvent(int id)
        {
            try
            {
                EventRequirements objList = _customerDbContext.EventRequirements.First(u => u.EventRequirementId == id);
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
