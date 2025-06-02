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
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public CustomerController(CustomerDbContext customerDbContext,IMapper mapper)
        {
            _customerDbContext = customerDbContext;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetCustomer()
        {
            try
            {
                IEnumerable<Customer> objList = _customerDbContext.Customers.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<CustomerDto>>(objList);
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
        public ResponseDto GetCustomer(string id)
        {
            try
            {
                Customer objList = _customerDbContext.Customers.First(u => u.CustomerId == id);
                _responseDto.Result = _mapper.Map<CustomerDto>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto AddCustomerDetails([FromBody] CustomerDto customerDto)
        {
            try
            {
                Customer obj = _mapper.Map<Customer>(customerDto);
                obj.CreatedAt = DateTime.UtcNow;

                _customerDbContext.Customers.Add(obj);
                _customerDbContext.SaveChanges();

                _responseDto.Result = _mapper.Map<CustomerDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }


        [HttpPut]
        public ResponseDto UpdateCustomerDetails([FromBody] CustomerDto customerDto)
        {
            try
            {
                Customer obj = _mapper.Map<Customer>(customerDto);
                _customerDbContext.Customers.Update(obj);
                _customerDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<CustomerDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess= false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Route("{id}")]
        public ResponseDto DeleteCustomer(string id)
        {
            try
            {
                Customer objList = _customerDbContext.Customers.First(u => u.CustomerId == id);
                _customerDbContext.Remove(objList);
                _customerDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess= false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
            
        }
    }
}
