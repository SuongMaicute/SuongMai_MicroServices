using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suongmai.Services.ShoppingCartAPI.Data;
using Suongmai.Services.ShoppingCartAPI.Models;
using Suongmai.Services.ShoppingCartAPI.Models.Dto;
using System;

namespace Suongmai.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly CartDBContext _db;
        /*private IProductService _productService;
        private ICouponService _couponService;
        private IConfiguration _configuration;
        private readonly IMessageBus _messageBus;*/
        public ShoppingCartAPIController(CartDBContext db,IMapper mapper)
        {
            _db = db;            
            this._response = new ResponseDto();
            _mapper = mapper;
            
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cart) 
        {
            try
            {
                var CartHeaderDB = await _db.CarHeaders.FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
                if (CartHeaderDB == null)
                {
                    // create new cart 
                }
                else
                {
                    var cartDetalDb = await _db.CarDeatails.FirstOrDefaultAsync(u => u.ProductId == cart.CartDetails.First().ProductId && 
                    u.CartHeaderId == CartHeaderDB.CartHeaderId);

                    if (cartDetalDb == null)
                    {
                        // create new cart detail
                        CartHeader cartHeader = _mapper.Map<CartHeader>(cart.CartHeader);
                        _db.CarHeaders.Add(cartHeader);
                        _db.SaveChanges();
                        cart.CartDetails.First().CartHeaderId =  cartHeader.CartHeaderId;
                        _db.CarDeatails.Add(_mapper.Map<CartDetail>(cart.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // update count in cart detail
                    }

                }


            } catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }

            return _response;
        }

    }
}
