
using AuthAPI.Data;

using AuthAPI.Models;

using AuthAPI.Models.Dto;

using AuthAPI.Service.IService;

using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Service

{

    public class AuthService : IAuthService

    {

        private readonly AppDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator, RoleManager<IdentityRole> roleManager)

        {

            _db = db;

            _userManager = userManager;

            _jwtTokenGenerator = jwtTokenGenerator;

            _roleManager = roleManager;

        }

        public async Task<List<UserDto>> GetAllUsers()

        {

            var users = _userManager.Users.ToList(); // Or await _userManager.Users.ToListAsync();

            var userDtoList = new List<UserDto>();

            foreach (var user in users)

            {

                var roles = await _userManager.GetRolesAsync(user);

                userDtoList.Add(new UserDto

                {

                    ID = user.Id,

                    Email = user.Email,

                    Name = user.UserName,

                    PhoneNumber = user.PhoneNumber,

                    Roles = roles.ToList() // ✅ Set the roles

                });

            }

            return userDtoList;

        }

        public async Task<bool> AssignRole(string email, string roleName)

        {

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user != null)

            {

                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())

                {

                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                }

                await _userManager.AddToRoleAsync(user, roleName);

                return true;

            }

            return false;

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)

        {

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Username.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)

            {

                return new LoginResponseDto()

                {

                    User = null,

                    Token = ""

                };

            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new()

            {

                Email = user.Email,

                ID = user.Id,

                Name = user.Name,

                PhoneNumber = user.PhoneNumber

            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()

            {

                User = userDto,

                Token = token,

                Roles = roles.ToList()

            };

            return loginResponseDto;

        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto, string role)

        {

            ApplicationUser user = new()

            {

                UserName = registrationRequestDto.Email,

                Email = registrationRequestDto.Email,

                NormalizedEmail = registrationRequestDto.Email,

                Name = registrationRequestDto.Name,

                PhoneNumber = registrationRequestDto.PhoneNumber

            };

            try

            {

                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)

                {

                    if (!await _roleManager.RoleExistsAsync(role))

                    {

                        await _roleManager.CreateAsync(new IdentityRole(role));

                    }

                    await _userManager.AddToRoleAsync(user, role);

                    var userToReturn = _db.ApplicationUsers.First(u =>

                    u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()

                    {

                        Email = userToReturn.Email,

                        ID = userToReturn.Id,

                        Name = userToReturn.Name,

                        PhoneNumber = userToReturn.PhoneNumber

                    };

                    return "";

                }

                else

                {

                    return result.Errors.FirstOrDefault().Description ?? "Unknown Error Occurred";

                }

            }

            catch (Exception ex)

            {

            }

            return "Error encountered : ";

        }

    }

}

