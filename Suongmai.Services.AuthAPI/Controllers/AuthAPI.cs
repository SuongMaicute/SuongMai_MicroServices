using Mango.MessageBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Suongmai.Services.AuthAPI.Models.Dto;
using Suongmai.Services.AuthAPI.Service.IService;

namespace Suongmai.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPI : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        protected ResponseDto _response;

        public AuthAPI( IAuthService _service, IMessageBus mess, IConfiguration config)
        {
            _authService = _service;
            _configuration = config;
            _messageBus = mess;
            _response = new ResponseDto();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {

            var LoginRespone = await _authService.Login(model);
            if (LoginRespone.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }

            _response.result = LoginRespone;

            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {

            var AssignOK = await _authService.AssingRole(model.Email, model.Role.ToUpper());
            if (!AssignOK)
            {
                _response.IsSuccess = false;
                _response.Message = "User already exist !!!";
                return BadRequest(_response);
            }

           
            return Ok(_response);
        }

    }
}
