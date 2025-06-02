using AuthAPI.Models.Dto;

namespace AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto,string role);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<bool> AssignRole(string email ,  string roleName);

        Task<List<UserDto>> GetAllUsers();

    }

}
