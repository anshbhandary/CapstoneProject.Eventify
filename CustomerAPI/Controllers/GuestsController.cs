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
    public class GuestsController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public GuestsController(CustomerDbContext customerDbContext, IMapper mapper)
        {
            _customerDbContext = customerDbContext;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetGuestDetails()
        {
            try
            {
                IEnumerable<GuestInfo> objList = _customerDbContext.GuestInfos.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<GuestInfo>>(objList);
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
        public ResponseDto GetGuestDetails(int id)
        {
            try
            {
                GuestInfo objList = _customerDbContext.GuestInfos.First(u => u.GuestId == id);
                _responseDto.Result = _mapper.Map<GuestInfo>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto AddGuestInfo([FromBody] GuestInfoDto GuestInfoDto)
        {
            try
            {
                GuestInfo obj = _mapper.Map<GuestInfo>(GuestInfoDto);
                _customerDbContext.GuestInfos.Add(obj);
                _customerDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<GuestInfo>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        public ResponseDto UpdateServiceDetails([FromBody] GuestInfoDto GuestInfoDto)
        {
            try
            {
                GuestInfo obj = _mapper.Map<GuestInfo>(GuestInfoDto);
                _customerDbContext.GuestInfos.Update(obj);
                _customerDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<GuestInfo>(obj);
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
        public ResponseDto DeleteGuestInfo(int id)
        {
            try
            {
                GuestInfo objList = _customerDbContext.GuestInfos.First(u => u.RequestId == id);
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
