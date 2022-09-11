using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos.Products;
using Service.Interfaces.Products;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Payments;
using WebApi.Infrastructures.Models.Paramaters;
using WebApi.Infrastructures.Models.ViewModels;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class ProductCategoryTypeController : CustomBaseController
    {
        private readonly IProductCategoryTypeService _productCategoryTypeService;
        private readonly IMapper _mapper;

        public ProductCategoryTypeController(IProductCategoryTypeService productCategoryTypeService,
            IMapper mapper)
        {
            _productCategoryTypeService = productCategoryTypeService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("api/productCategoryType")]
        public async Task<ActionResult<BaseResponse<List<ProductCategoryTypeDisplayDto>>>> GetProductCategoryType()
        {
            BaseResponse<List<ProductCategoryTypeDisplayDto>> baseResponse = new BaseResponse<List<ProductCategoryTypeDisplayDto>>();

            List<ProductCategoryTypeDto> categoryTypes = await _productCategoryTypeService.GetAllAsync();
            List<ProductCategoryTypeDisplayDto> displayModels = _mapper.Map<List<ProductCategoryTypeDisplayDto>>(categoryTypes);

            baseResponse.IsSuccess = true;
            baseResponse.Data = displayModels;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductCategoryTypeDisplayDto>>> GetProductCategoryType(int id)
        {
            BaseResponse<ProductCategoryTypeDisplayDto> baseResponse = new BaseResponse<ProductCategoryTypeDisplayDto>();

            ProductCategoryTypeDto productCategoryType = await _productCategoryTypeService.GetByIdAsync(id);

            if (productCategoryType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            ProductCategoryTypeDisplayDto displayDto = _mapper.Map<ProductCategoryTypeDisplayDto>(productCategoryType);

            baseResponse.IsSuccess = true;
            baseResponse.Data = displayDto;

            return baseResponse;
        }

        [HttpPut("api/admin/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> PutProductCategoryType(int id, BaseRequest<UpdateProductCategoryTypeParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            if (id != baseRequest.Data.Id)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "資料錯誤";

                return baseResponse;
            }

            bool isExists = await _productCategoryTypeService.IsExistsAsync(id);
            if (isExists == false)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            ProductCategoryTypeUpdateDto updateDto = _mapper.Map<ProductCategoryTypeUpdateDto>(baseRequest.Data);

            bool result = await _productCategoryTypeService.UpdateAsync(updateDto);

            if (result == true)
            {
                baseResponse.IsSuccess = true;
                baseResponse.Message = "修改成功";
            }
            else
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "修改失敗";
            }

            return baseResponse;
        }

        [HttpPost("api/admin/productCategoryType")]
        public async Task<ActionResult<BaseResponse<bool>>> PostProductCategoryType(BaseRequest<CreateProductCategoryTypeParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            ProductCategoryTypeCreateDto createDto = _mapper.Map<ProductCategoryTypeCreateDto>(baseRequest.Data);

            bool result = await _productCategoryTypeService.CreateAsync(createDto);

            if (result == true)
            {
                baseResponse.IsSuccess = true;
                baseResponse.Message = "建立成功";
            }
            else
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "建立失敗";
            }

            return baseResponse;
        }

        [HttpDelete("api/admin/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteProductCategoryType(int id)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            bool isExists = await _productCategoryTypeService.IsExistsAsync(id);
            if (isExists == false)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            bool result = await _productCategoryTypeService.DeleteByIdAsync(id);

            if (result == true)
            {
                baseResponse.IsSuccess = true;
                baseResponse.Message = "刪除成功";
            }
            else
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "刪除失敗";
            }

            return baseResponse;
        }
    }
}
