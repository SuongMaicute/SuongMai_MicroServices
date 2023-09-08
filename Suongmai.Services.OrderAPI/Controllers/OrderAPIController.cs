using AutoMapper;
using Mango.MessageBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Suongmai.Services.OrderAPI.Models;
using Suongmai.Services.OrderAPI.Models.Dto;
using Suongmai.Services.OrderAPI.Service.IService;
using Suongmai.Services.OrderAPI.Util;
using Suongmai.Services.ShoppingCartAPI.Data;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Suongmai.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly OrderDBContext _db;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public OrderAPIController(OrderDBContext db,
            IProductService productService, IMapper mapper, IConfiguration configuration
            , IMessageBus messageBus)
        {
            _db = db;
            _messageBus = messageBus;
            this._response = new ResponseDto();
            _productService = productService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.Details = _mapper.Map<IEnumerable<OrderDetailDto>>(cartDto.CartDetails);


                OrderHeader orderCreated =  _db.CarHeOrderHeadersOrderHeadersaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;

                await _db.SaveChangesAsync();
                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.result = orderHeaderDto;
            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message; 
            }
            return _response; 
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {
               

                var options = new SessionCreateOptions
                {
                    SuccessUrl = "https://example.com/success",
                    LineItems = new List<SessionLineItemOptions>
                  {
                    new SessionLineItemOptions
                    {
                      Price = "price_H5ggYwtDq4fbrJ",
                      Quantity = 2,
                    },
                  },
                    Mode = "payment",
                };
                var service = new SessionService();
                service.Create(options);
            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



    }
}
