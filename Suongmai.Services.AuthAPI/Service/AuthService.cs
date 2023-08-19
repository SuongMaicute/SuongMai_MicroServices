﻿using Microsoft.AspNetCore.Identity;
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

        public AuthService( AuthAPIContext db,
            UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _useManager = usermanager;
            _roleManager = roleManager;
            
        }

        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
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

            };
            return  "Error encounter";
        }
    }
}
