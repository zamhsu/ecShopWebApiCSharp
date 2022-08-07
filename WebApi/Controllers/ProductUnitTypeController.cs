using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.Products;
using Service.Interfaces.Products;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Products;
using WebApi.Infrastructures.Models.InputParamaters;
using WebApi.Infrastructures.Models.OutputModels;

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
        public async Task<ActionResult<BaseResponse<List<ProductUnitTypeDisplayDto>>>> GetProductUnitType()
        {
            BaseResponse<List<ProductUnitTypeDisplayDto>> baseResponse = new BaseResponse<List<ProductUnitTypeDisplayDto>>();

            List<ProductUnitType> unitTypes = await _productUnitTypeService.GetAllAsync();

            baseResponse.IsSuccess = true;
            baseResponse.Data = _mapper.Map<List<ProductUnitTypeDisplayDto>>(unitTypes);

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/productUnitType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductUnitTypeDisplayDto>>> GetProductUnitType(int id)
        {
            BaseResponse<ProductUnitTypeDisplayDto> baseResponse = new BaseResponse<ProductUnitTypeDisplayDto>();

            ProductUnitType productUnitType = await _productUnitTypeService.GetByIdAsync(id);

            if (productUnitType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = _mapper.Map<ProductUnitTypeDisplayDto>(productUnitType);

            return baseResponse;
        }

        [HttpPut("api/admin/productUnitType/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> PutProductUnitType(int id, BaseRequest<UpdateProductUnitTypeParameter> baseRequest)
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
        public async Task<ActionResult<BaseResponse<bool>>> PostProductUnitType(BaseRequest<CreateProductUnitTypeParameter> baseRequest)
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
