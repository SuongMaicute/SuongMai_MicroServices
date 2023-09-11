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
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    
                };
               


                var DiscountsObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions
                    {
                        Coupon=stripeRequestDto.OrderHeader.CouponCode
                    }
                };

                foreach (var item in stripeRequestDto.OrderHeader.Details)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // $20.99 -> 2099
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                if (stripeRequestDto.OrderHeader.Discount > 0)
                {
                    options.Discounts = DiscountsObj;
                }
                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.CarHeOrderHeadersOrderHeadersaders.First(u => u.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionID = session.Id;
                _db.SaveChanges(); 
                _response.result = stripeRequestDto;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


        [Authorize]
        [HttpPost("ValidateStripe")]
        public async Task<ResponseDto> ValidateStripe([FromBody]  int orderHeaderID)
        {
            try
            {
                OrderHeader orderHeader = _db.CarHeOrderHeadersOrderHeadersaders.First(
                    u=>u.OrderHeaderId == orderHeaderID
                    ) ;

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionID);
                
                var peymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = peymentIntentService.Get(session.PaymentIntentId);

                if(paymentIntent.Status == "succeeded")
                {
                    // payment OK
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _db.SaveChanges();

                    RewardsDto reward = new() { 
                      OrderId = orderHeader.OrderHeaderId,
                      RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                      UserId = orderHeader.UserId
                    };
                    string topic_name = _configuration.GetValue<string>("TopicQueueName:OrderCreatedTopic");
                    await _messageBus.PublishMessage(reward,topic_name);

                    _response.result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }

               

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


    }
}
