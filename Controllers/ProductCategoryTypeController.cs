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
    public class ProductCategoryTypeController : ControllerBase
    {
        private readonly IProductCategoryTypeService _productCategoryTypeService;

        public ProductCategoryTypeController(IProductCategoryTypeService productCategoryTypeService)
        {
            _productCategoryTypeService = productCategoryTypeService;
        }

        [HttpGet("api/productCategoryType")]
        public ActionResult<BaseResponse<List<ProductCategoryType>>> GetProductCategoryType()
        {
            BaseResponse<List<ProductCategoryType>> baseResponse = new BaseResponse<List<ProductCategoryType>>();

            baseResponse.IsSuccess = true;
            baseResponse.Data = _productCategoryTypeService.GetAll();

            return baseResponse;
        }

        [HttpGet("api/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductCategoryType>>> GetProductCategoryType(int id)
        {
            BaseResponse<ProductCategoryType> baseResponse = new BaseResponse<ProductCategoryType>();

            ProductCategoryType productCategoryType = await _productCategoryTypeService.GetByIdAsync(id);

            if (productCategoryType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = productCategoryType;

            return baseResponse;
        }

        [HttpPut("api/admin/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductCategoryType>>> PutProductCategoryType(int id, BaseRequest<UpdateProductCategoryTypeModel> baseRequest)
        {
            BaseResponse<ProductCategoryType> baseResponse = new BaseResponse<ProductCategoryType>();

            ProductCategoryType productCategoryType = await _productCategoryTypeService.GetByIdAsync(id);
            if (productCategoryType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _productCategoryTypeService.UpdateAsync(id, baseRequest.Data);

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

        [HttpPost("api/admin/productCategoryType")]
        public async Task<ActionResult<BaseResponse<ProductCategoryType>>> PostProductCategoryType(BaseRequest<CreateProductCategoryTypeModel> baseRequest)
        {
            BaseResponse<ProductCategoryType> baseResponse = new BaseResponse<ProductCategoryType>();

            try
            {
                await _productCategoryTypeService.CreateAsync(baseRequest.Data);

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

        [HttpDelete("api/admin/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductCategoryType>>> DeleteProductCategoryType(int id)
        {
            BaseResponse<ProductCategoryType> baseResponse = new BaseResponse<ProductCategoryType>();

            ProductCategoryType productCategoryType = await _productCategoryTypeService.GetByIdAsync(id);
            if (productCategoryType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _productCategoryTypeService.DeleteByIdAsync(id);

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
