using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suongmai.Services.ShoppingCartAPI.Data;
using Suongmai.Services.ShoppingCartAPI.Models;
using Suongmai.Services.ShoppingCartAPI.Models.Dto;
using Suongmai.Services.ShoppingCartAPI.Service.IService;
using System;
using System.Globalization;

namespace Suongmai.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly CartDBContext _db;
        private IProductService _productService;
        /*private ICouponService _couponService;
        private IConfiguration _configuration;
        private readonly IMessageBus _messageBus;*/
        public ShoppingCartAPIController(CartDBContext db,IMapper mapper, IProductService productService)
        {
            _db = db;            
            this._response = new ResponseDto();
            _mapper = mapper;
            _productService = productService;
            
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto) 
        {
            try
            {
                var cartHeaderFromDb = await _db.CarHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CarHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CarDeatails.Add(_mapper.Map<CartDetail>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CarDeatails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CarDeatails.Add(_mapper.Map<CartDetail>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CarDeatails.Update(_mapper.Map<CartDetail>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("GetCart/{userID}")]
        public async Task<ResponseDto> GetCart(String userID)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CarHeaders.First(u => u.UserId == userID))

                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailDto>>(_db.CarDeatails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));
                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach(var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);    
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                _response.result = cart;
            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }
            return _response;
        }


            [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int CartDetailsId)
        {
            try
            {
                CartDetail cartDetail = _db.CarDeatails.First(u => u.CartDetailsId == CartDetailsId);

                int totalCount = _db.CarDeatails.Where(u=> u.CartHeaderId == cartDetail.CartHeaderId).Count();

                if (totalCount ==1) {
                    CartHeader cartHeaderRemove = await _db.CarHeaders.FirstOrDefaultAsync(u => u.CartHeaderId==cartDetail.CartHeaderId);
                    _db.CarHeaders.Remove(cartHeaderRemove);
                   
                }
                await _db.SaveChangesAsync();
                

                _response.result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }





    }
}
