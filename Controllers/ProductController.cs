using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Base.IServices.Products;
using WebApi.Dtos;
using WebApi.Dtos.Products;
using WebApi.Models;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("api/product")]
        public ActionResult<BaseResponse<List<Product>>> GetProduct()
        {
            BaseResponse<List<Product>> baseResponse = new BaseResponse<List<Product>>();

            baseResponse.IsSuccess = true;
            baseResponse.Data = _productService.GetAllUsable();

            return baseResponse;
        }

        [HttpGet("api/product/{guid}")]
        public async Task<ActionResult<BaseResponse<Product>>> GetProduct(string guid)
        {
            BaseResponse<Product> baseResponse = new BaseResponse<Product>();

            Product product = await _productService.GetByGuidAsync(guid);

            if (product == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = product;

            return baseResponse;
        }

        [HttpPut("api/admin/product/{guid}")]
        public async Task<ActionResult<BaseResponse<Product>>> PutProduct(string guid, BaseRequest<UpdateProductModel> baseRequest)
        {
            BaseResponse<Product> baseResponse = new BaseResponse<Product>();

            Product product = await _productService.GetByGuidAsync(guid);
            if (product == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _productService.UpdateAsync(guid, baseRequest.Data, baseRequest.UserTimeZone);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "修改成功";
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "修改失敗";
            }

            return baseResponse;
        }

        [HttpPost("api/admin/product")]
        public async Task<ActionResult<BaseResponse<Product>>> PostProduct(BaseRequest<CreateProductModel> baseRequest)
        {
            BaseResponse<Product> baseResponse = new BaseResponse<Product>();

            try
            {
                await _productService.CreateAsync(baseRequest.Data, baseRequest.UserTimeZone);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "建立成功";
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "建立失敗";
            }

            return baseResponse;
        }

        [HttpDelete("api/admin/product/{guid}")]
        public async Task<ActionResult<BaseResponse<Product>>> DeleteProduct(string guid)
        {
            BaseResponse<Product> baseResponse = new BaseResponse<Product>();

            Product product = await _productService.GetByGuidAsync(guid);
            if (product == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _productService.DeleteByGuidAsync(guid);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "刪除成功";
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "刪除失敗";
            }

            return baseResponse;
        }
    }
}
