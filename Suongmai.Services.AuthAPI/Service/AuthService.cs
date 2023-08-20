using Microsoft.AspNetCore.Identity;
using Suongmai.Services.AuthAPI.Data;
using Suongmai.Services.AuthAPI.Models;
using Suongmai.Services.AuthAPI.Models.Dto;
using Suongmai.Services.AuthAPI.Service.IService;
using System.Reflection.Metadata.Ecma335;

namespace Suongmai.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AuthAPIContext _db;
        private readonly UserManager<ApplicationUser> _useManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService( AuthAPIContext db,IJwtTokenGenerator Jwt,
            UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _useManager = usermanager;
            _roleManager = roleManager;
            _jwtTokenGenerator = Jwt;
            
        }

        public async Task<bool> AssingRole(string email, string roleName)
        {

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (! _roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    // create role if not exist 
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                };
                await _useManager.AddToRoleAsync(user, roleName);
                return true;

            }
            return false;

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Username.ToLower() );
            bool isvalid = await _useManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || isvalid == false)
            {
                return new LoginResponseDto() { 
                    User = null, Token =""
                };
            }
            // if user was found, generate jwt token

            var token = _jwtTokenGenerator.GenerateToken(user);
            UserDto userDto = new UserDto()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponse = new LoginResponseDto()
            {
                User = userDto,
                Token =token
            };

            return loginResponse;
        
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _useManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.Name == registrationRequestDto.Email);
                    UserDto userDto = new UserDto()
                    {
                        Email = userToReturn.Email,
                        ID= userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber=userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception e)
            {
                return  "";
            };
        }
    }
}
