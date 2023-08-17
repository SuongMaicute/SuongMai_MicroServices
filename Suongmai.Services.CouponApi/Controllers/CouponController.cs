using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suongmai.Services.CouponApi.Data;
using Suongmai.Services.CouponApi.Models;
using Suongmai.Services.CouponApi.Models.Dto;

namespace Suongmai.Services.CouponApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly AppDBContext _db;
        private ResponseDto _respone;
        private IMapper _mapper;

        public CouponController(AppDBContext DB, IMapper mapper)
        {
            _db = DB;   
            _respone = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _respone.result = _mapper.Map<IEnumerable<CouponDto>>(objList);

            }catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;
            }
            return _respone;
        
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First( o =>o.CouponId ==id);
                _respone.result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;

            }
            return _respone;

        }
    }
}
