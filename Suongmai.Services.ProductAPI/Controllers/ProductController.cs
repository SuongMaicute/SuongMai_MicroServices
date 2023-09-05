using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suongmai.Services.ProductAPI.Data;
using Suongmai.Services.ProductAPI.Models;
using Suongmai.Services.ProductAPI.Models.Dto;

namespace Suongmai.Services.ProductApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly ProductDBContext _db;
        private ResponseDto _respone;
        private IMapper _mapper;

        public ProductController(ProductDBContext DB, IMapper mapper)
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
                IEnumerable<Product> objList = _db.Products.ToList();
                _respone.result = _mapper.Map<IEnumerable<ProductDto>>(objList);

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
                Product obj = _db.Products.FirstOrDefault( o =>o.ProductId ==id);
                _respone.result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;

            }
            return _respone;

        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public ResponseDto GetBycode(String name)
        {
            try
            {
                Product obj = _db.Products.First(o => o.Name.ToLower() == name.ToLower());
                _respone.result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;

            }
            return _respone;

        }

        [HttpPost]
		[Authorize(Roles = "ADMIN")]
		public ResponseDto Create([FromBody] Product ProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
               _db.Products.Add(obj);
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
		[Authorize(Roles = "ADMIN")]
		public ResponseDto update([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
                _db.Products.Update(obj);
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
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(o => o.ProductId == id);
                _respone.result = _mapper.Map<ProductDto>(obj);

                _db.Products.Remove(obj);
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
