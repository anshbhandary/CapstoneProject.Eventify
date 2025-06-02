using AutoMapper;
using EventManagingAPI.Models.Dto;
using EventManagingAPI.Models;
using EventManagingAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventManagingAPI.Data;
using Azure;

namespace EventManagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventManagingController : ControllerBase
    {
        private readonly IManagedEventRepository _managedEventRepository;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public EventManagingController(IManagedEventRepository managedEventRepository, IMapper mapper)
        {
            _managedEventRepository = managedEventRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                var events = _managedEventRepository.GetAll();
                _responseDto.Result = _mapper.Map<IEnumerable<ManagedEventResponseDTO>>(events);
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpGet("{id:int}")]
        public ResponseDto GetById(int id)
        {
            try
            {
                var managedEvent = _managedEventRepository.GetById(id);

                if (managedEvent == null)
                {
                    _responseDto.Result = "No Event Found";
                }
                else
                {
                    _responseDto.Result = _mapper.Map<ManagedEventResponseDTO>(managedEvent);
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto CreateEvent([FromBody] ManagedEventDTO managedEventDto)
        {
            try
            {
                var managedEvent = _mapper.Map<ManagedEvent>(managedEventDto);
                _managedEventRepository.Add(managedEvent);
                _managedEventRepository.SaveChanges();

                _responseDto.Result = _mapper.Map<ManagedEventResponseDTO>(managedEvent);
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto DeleteById(int id)
        {
            try
            {
                var managedEvent = _managedEventRepository.GetById(id);
                _managedEventRepository.Remove(managedEvent);

            }
            catch (Exception ex) 
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

    }
}
