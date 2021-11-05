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
    public class ProductUnitTypeController : ControllerBase
    {
        private readonly IProductUnitTypeService _productUnitTypeService;

        public ProductUnitTypeController(IProductUnitTypeService productUnitTypeService)
        {
            _productUnitTypeService = productUnitTypeService;
        }

        [HttpGet("api/productUnitType")]
        public ActionResult<BaseResponse<List<ProductUnitType>>> GetProductUnitType()
        {
            BaseResponse<List<ProductUnitType>> baseResponse = new BaseResponse<List<ProductUnitType>>();

            baseResponse.IsSuccess = true;
            baseResponse.Data = _productUnitTypeService.GetAll();

            return baseResponse;
        }

        [HttpGet("api/productUnitType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductUnitType>>> GetProductUnitType(int id)
        {
            BaseResponse<ProductUnitType> baseResponse = new BaseResponse<ProductUnitType>();

            ProductUnitType productUnitType = await _productUnitTypeService.GetByIdAsync(id);

            if (productUnitType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = productUnitType;

            return baseResponse;
        }

        [HttpPut("api/admin/productUnitType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductUnitType>>> PutProductUnitType(int id, BaseRequest<UpdateProductUnitTypeModel> baseRequest)
        {
            BaseResponse<ProductUnitType> baseResponse = new BaseResponse<ProductUnitType>();

            ProductUnitType productUnitType = await _productUnitTypeService.GetByIdAsync(id);
            if (productUnitType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _productUnitTypeService.UpdateAsync(id, baseRequest.Data);

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

        [HttpPost("api/admin/productUnitType")]
        public async Task<ActionResult<BaseResponse<ProductUnitType>>> PostProductUnitType(BaseRequest<CreateProductUnitTypeModel> baseRequest)
        {
            BaseResponse<ProductUnitType> baseResponse = new BaseResponse<ProductUnitType>();

            try
            {
                await _productUnitTypeService.CreateAsync(baseRequest.Data);

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

        [HttpDelete("api/admin/productUnitType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductUnitType>>> DeleteProductUnitType(int id)
        {
            BaseResponse<ProductUnitType> baseResponse = new BaseResponse<ProductUnitType>();

            ProductUnitType productUnitType = await _productUnitTypeService.GetByIdAsync(id);
            if (productUnitType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _productUnitTypeService.DeleteByIdAsync(id);

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
