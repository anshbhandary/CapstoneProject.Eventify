using AutoMapper;
using Azure;
using EventManagingAPI.Models;
using EventManagingAPI.Models.Dto;
using EventManagingAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public ToDoListController(IToDoItemRepository toDoItemRepository, IMapper mapper)
        {
            _toDoItemRepository = toDoItemRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        [Route("Event/{id:int}")]
        public ResponseDto GetByEventId(int id)
        {
            try
            {
                var items = _toDoItemRepository.GetByManagedEventId(id);
                _responseDto.Result = _mapper.Map<IEnumerable<ToDoItemResponseDTO>>(items);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Route("Event")]
        public ResponseDto CreateNew([FromBody] ToDoItemDTO toDoItemDTO)
        {
            try
            {
                var toDoItem = _mapper.Map<ToDoItem>(toDoItemDTO);
                _toDoItemRepository.Add(toDoItem);
                _toDoItemRepository.SaveChanges();

                _responseDto.Result = _mapper.Map<ToDoItemResponseDTO>(toDoItem);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Route("Event/{id:int}")]
        public ResponseDto UpdateToDo(int id, [FromBody] ToDoItemUpdateDTO toDoItemUpdateDTO)
        {
            try
            {
                var existingItem = _toDoItemRepository.GetById(id);
                if (existingItem == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Item not found";
                }

                _mapper.Map(toDoItemUpdateDTO, existingItem);
                _toDoItemRepository.Update(existingItem);
                _toDoItemRepository.SaveChanges();

                _responseDto.Result = _mapper.Map<ToDoItemResponseDTO>(existingItem);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }


        [HttpDelete]
        [Route("Event/{id:int}")]
        public ResponseDto DeleteToDo(int id)
        {
            try
            {
                var item =_toDoItemRepository.GetById(id);
                _toDoItemRepository.Remove(item);
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

       

