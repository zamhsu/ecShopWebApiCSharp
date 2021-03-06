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
using Microsoft.AspNetCore.Authorization;
using WebApi.Core;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class ProductUnitTypeController : CustomBaseController
    {
        private readonly IProductUnitTypeService _productUnitTypeService;
        private readonly IMapper _mapper;

        public ProductUnitTypeController(IProductUnitTypeService productUnitTypeService,
            IMapper mapper)
        {
            _productUnitTypeService = productUnitTypeService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("api/productUnitType")]
        public async Task<ActionResult<BaseResponse<List<ProductUnitTypeDisplayModel>>>> GetProductUnitType()
        {
            BaseResponse<List<ProductUnitTypeDisplayModel>> baseResponse = new BaseResponse<List<ProductUnitTypeDisplayModel>>();

            List<ProductUnitType> unitTypes = await _productUnitTypeService.GetAllAsync();

            baseResponse.IsSuccess = true;
            baseResponse.Data = _mapper.Map<List<ProductUnitTypeDisplayModel>>(unitTypes);

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/productUnitType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductUnitTypeDisplayModel>>> GetProductUnitType(int id)
        {
            BaseResponse<ProductUnitTypeDisplayModel> baseResponse = new BaseResponse<ProductUnitTypeDisplayModel>();

            ProductUnitType productUnitType = await _productUnitTypeService.GetByIdAsync(id);

            if (productUnitType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = _mapper.Map<ProductUnitTypeDisplayModel>(productUnitType);

            return baseResponse;
        }

        [HttpPut("api/admin/productUnitType/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> PutProductUnitType(int id, BaseRequest<UpdateProductUnitTypeModel> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            if (id != baseRequest.Data.Id)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "資料錯誤";

                return baseResponse;
            }

            ProductUnitType existedProductUnitType = await _productUnitTypeService.GetByIdAsync(id);
            if (existedProductUnitType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            ProductUnitType productUnitType = _mapper.Map<ProductUnitType>(baseRequest.Data);

            try
            {
                await _productUnitTypeService.UpdateAsync(id, productUnitType);

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
        public async Task<ActionResult<BaseResponse<bool>>> PostProductUnitType(BaseRequest<CreateProductUnitTypeModel> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            ProductUnitType productUnitType = _mapper.Map<ProductUnitType>(baseRequest.Data);

            try
            {
                await _productUnitTypeService.CreateAsync(productUnitType);

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
        public async Task<ActionResult<BaseResponse<bool>>> DeleteProductUnitType(int id)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

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
