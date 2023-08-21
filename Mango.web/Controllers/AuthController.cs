using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController( IAuthService service )
        {
            _authService = service;
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
    }
}
