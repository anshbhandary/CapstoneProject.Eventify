
using AuthAPI.Models;

using AuthAPI.Models.Dto;

using AuthAPI.Service.IService;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers

{

    [Route("api/auth")]

    [ApiController]

    public class AuthApiController : ControllerBase

    {

        private readonly IAuthService _authService;

        private readonly UserManager<ApplicationUser> _userManager;

        protected ResponseDto _response;

        public AuthApiController(IAuthService authService, UserManager<ApplicationUser> userManager)

        {

            _authService = authService;

            _userManager = userManager;

            _response = new();

        }

        //[HttpPost("register")]

        //public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)

        //{

        // var errorMessage = await _authService.Register(model);

        // if (!string.IsNullOrEmpty(errorMessage))

        // {

        // _response.IsSuccess = false;

        // _response.Message= errorMessage;

        // return BadRequest(_response);

        // }

        // return Ok(_response);

        //}

        [HttpPost("register/customer")]

        public async Task<IActionResult> RegisterCustomer([FromBody] RegistrationRequestDto model)

        {

            var errorMessage = await _authService.Register(model, "Customer");

            if (!string.IsNullOrEmpty(errorMessage))

            {

                _response.IsSuccess = false;

                _response.Message = errorMessage;

                return BadRequest(_response);

            }

            return Ok(_response);

        }

        [HttpPost("register/vendor")]

        public async Task<IActionResult> RegisterVendor([FromBody] RegistrationRequestDto model)

        {

            var errorMessage = await _authService.Register(model, "Vendor");

            if (!string.IsNullOrEmpty(errorMessage))

            {

                _response.IsSuccess = false;

                _response.Message = errorMessage;

                return BadRequest(_response);

            }

            return Ok(_response);

        }

        [HttpPost("register/admin")]

        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)

        {

            var errorMessage = await _authService.Register(model, "Admin");

            if (!string.IsNullOrEmpty(errorMessage))

            {

                _response.IsSuccess = false;

                _response.Message = errorMessage;

                return BadRequest(_response);

            }

            return Ok(_response);

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)

        {

            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)

            {

                _response.IsSuccess = false;

                _response.Message = "Username or password is incorrect";

                return BadRequest(_response);

            }

            _response.Result = loginResponse;

            return Ok(_response);

        }

        [HttpGet("users")]

        public async Task<IActionResult> GetAllUsers()

        {

            try

            {

                var users = await _authService.GetAllUsers();

                _response.Result = users;

                _response.IsSuccess = true;

                return Ok(_response);

            }

            catch (Exception ex)

            {

                _response.IsSuccess = false;

                _response.Message = ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }

        }

        //[HttpDelete("delete-by-email")]

        //public async Task<IActionResult> DeleteUserByEmail([FromQuery] string email)

        //{

        // var user = await _userManager.FindByEmailAsync(email);

        // if (user == null)

        // {

        // _response.IsSuccess = false;

        // _response.Message = "User not found";

        // return NotFound(_response);

        // }

        // var result = await _userManager.DeleteAsync(user);

        // if (result.Succeeded)

        // {

        // _response.IsSuccess = true;

        // _response.Message = "User deleted successfully";

        // return Ok(_response);

        // }

        // _response.IsSuccess = false;

        // _response.Message = string.Join(", ", result.Errors.Select(e => e.Description));

        // return BadRequest(_response);

        //}

        //[HttpPost("AssignRole")]

        //public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)

        //{

        // var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());

        // if (!assignRoleSuccessful)

        // {

        // _response.IsSuccess = false;

        // _response.Message = "Error Encountered";

        // return BadRequest(_response);

        // }

        // return Ok(_response);

        //}

    }

}