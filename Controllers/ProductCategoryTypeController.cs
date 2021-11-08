using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductCategoryTypeController(IProductCategoryTypeService productCategoryTypeService,
            IMapper mapper)
        {
            _productCategoryTypeService = productCategoryTypeService;
            _mapper = mapper;
        }

        [HttpGet("api/productCategoryType")]
        public async Task<ActionResult<BaseResponse<List<ProductCategoryType>>>> GetProductCategoryType()
        {
            BaseResponse<List<ProductCategoryType>> baseResponse = new BaseResponse<List<ProductCategoryType>>();

            baseResponse.IsSuccess = true;
            baseResponse.Data = await _productCategoryTypeService.GetAllAsync();

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

            ProductCategoryType existedProductCategoryType = await _productCategoryTypeService.GetByIdAsync(id);
            if (existedProductCategoryType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            ProductCategoryType productCategoryType = _mapper.Map<ProductCategoryType>(baseRequest.Data);

            try
            {
                await _productCategoryTypeService.UpdateAsync(id, productCategoryType);

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

            ProductCategoryType productCategoryType = _mapper.Map<ProductCategoryType>(baseRequest.Data);

            try
            {
                await _productCategoryTypeService.CreateAsync(productCategoryType);

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
