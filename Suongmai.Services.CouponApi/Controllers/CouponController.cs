using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suongmai.Services.CouponApi.Data;
using Suongmai.Services.CouponApi.Models;
using Suongmai.Services.CouponApi.Models.Dto;

namespace Suongmai.Services.CouponApi.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly CouponDBContext _db;
        private ResponseDto _respone;
        private IMapper _mapper;

        public CouponController(CouponDBContext DB, IMapper mapper)
        {
            _db = DB;   
            _respone = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
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

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetBycode(String code)
        {
            try
            {
                Coupon obj = _db.Coupons.First(o => o.CouponCode.ToLower() == code.ToLower());
                _respone.result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;

            }
            return _respone;

        }

        [HttpPost]
        public ResponseDto Create([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
               _db.Coupons.Add(obj);
                _db.SaveChanges();

                _respone.result = obj;
                _respone.Message = "Create successfully!!!";
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;

            }
            return _respone;

        }

        [HttpPut]
        public ResponseDto update([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(obj);
                _db.SaveChanges();

                _respone.result = obj;
                _respone.Message = "Update successfully!!!";
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;

            }
            return _respone;

        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(o => o.CouponId == id);
                _respone.result = _mapper.Map<CouponDto>(obj);

                _db.Coupons.Remove(obj);
                _db.SaveChanges();
                _respone.Message = "Delete successfully";
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
