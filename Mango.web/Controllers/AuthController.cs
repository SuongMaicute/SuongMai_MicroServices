using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController( IAuthService service, ITokenProvider token)
        {
            _authService = service;
            _tokenProvider = token;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View( loginRequestDto );
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {

            ResponseDto respone = await _authService.LoginAsync(obj);
            
            if (respone != null && respone.IsSuccess)
            {
                LoginResponseDto loginResponseDto =

                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(respone.result));
                await SignInUser(loginResponseDto);
                var token = loginResponseDto.Token;
                _tokenProvider.SetToken(token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = respone.Message;
                return View(obj);
            }

        }

        [HttpGet]
        public IActionResult Register()
        {
            var RoleList = new List<SelectListItem>() {

                new SelectListItem() { Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem() { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            ViewBag.RoleList = RoleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto respone = await _authService.RegisterAsync(obj );
            ResponseDto assignRole;
            if (respone != null && respone.IsSuccess ) {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(obj);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Register succesffully !";
                    return RedirectToAction(nameof(Login));

                }

                var RoleList = new List<SelectListItem>() {

                new SelectListItem() { Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem() { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

                ViewBag.RoleList = RoleList;
            }
            
            return View(obj);
        }


        public IActionResult LogOut()
        {
            RegistrationRequestDto registrationRequestDto = new RegistrationRequestDto();

            return View();
        }

        private async Task SignInUser (LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
                jwt.Claims.FirstOrDefault( u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));


            var principal = new ClaimsPrincipal(identity);
              
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
