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
    public class ItemController : ControllerBase
    {
        private readonly IItemRequiredRepository _itemRequiredRepository;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public ItemController(IItemRequiredRepository itemRequiredRepository, IMapper mapper)
        {
            _itemRequiredRepository = itemRequiredRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet("{id:int}")]
        public ResponseDto GetByEventId(int id)
        {
            try
            {
                var items = _itemRequiredRepository.GetByManagedEventId(id);
                _responseDto.Result = _mapper.Map<IEnumerable<RequiredItemResponseDTO>>(items);
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
                var items = _itemRequiredRepository.GetAll();
                _responseDto.Result = _mapper.Map<IEnumerable<RequiredItemResponseDTO>>(items);
            }
            catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto CreateItem([FromBody] RequiredItemDTO requiredItemDto)
        {
            try
            {
                var requiredItem = _mapper.Map<ItemRequired>(requiredItemDto);
                _itemRequiredRepository.Add(requiredItem);
                _itemRequiredRepository.SaveChanges();

                _responseDto.Result = _mapper.Map<RequiredItemResponseDTO>(requiredItem);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut("{id:int}")]
        public ResponseDto UpdateItem(int id,[FromBody] RequiredItemUpdateDTO requiredItemUpdateDto)
        {
            try
            {
                var existingItem = _itemRequiredRepository.GetById(id);
                if (existingItem == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Item not found";
                }
                else
                {
                    _mapper.Map(requiredItemUpdateDto, existingItem);
                    _itemRequiredRepository.Update(existingItem);
                    _itemRequiredRepository.SaveChanges();

                    _responseDto.Result = _mapper.Map<RequiredItemResponseDTO>(existingItem);
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete("{id:int}")]
        public ResponseDto DeleteItem(int id)
        {
            try
            {
                var items = _itemRequiredRepository.GetById(id);
                _itemRequiredRepository.Remove(items);
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
