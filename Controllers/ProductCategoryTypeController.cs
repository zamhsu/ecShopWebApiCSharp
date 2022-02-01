using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Base.IServices.Products;
using WebApi.Dtos;
using WebApi.Dtos.Products;
using WebApi.Models.Products;
using Microsoft.AspNetCore.Authorization;
using WebApi.Core;

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
        public async Task<ActionResult<BaseResponse<List<ProductCategoryTypeDisplayModel>>>> GetProductCategoryType()
        {
            BaseResponse<List<ProductCategoryTypeDisplayModel>> baseResponse = new BaseResponse<List<ProductCategoryTypeDisplayModel>>();

            List<ProductCategoryType> categoryTypes = await _productCategoryTypeService.GetAllAsync();
            List<ProductCategoryTypeDisplayModel> displayModels = _mapper.Map<List<ProductCategoryTypeDisplayModel>>(categoryTypes);

            baseResponse.IsSuccess = true;
            baseResponse.Data = displayModels;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<ProductCategoryTypeDisplayModel>>> GetProductCategoryType(int id)
        {
            BaseResponse<ProductCategoryTypeDisplayModel> baseResponse = new BaseResponse<ProductCategoryTypeDisplayModel>();

            ProductCategoryType productCategoryType = await _productCategoryTypeService.GetByIdAsync(id);

            if (productCategoryType == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            ProductCategoryTypeDisplayModel displayModel = _mapper.Map<ProductCategoryTypeDisplayModel>(productCategoryType);

            baseResponse.IsSuccess = true;
            baseResponse.Data = displayModel;

            return baseResponse;
        }

        [HttpPut("api/admin/productCategoryType/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> PutProductCategoryType(int id, BaseRequest<UpdateProductCategoryTypeModel> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

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
        public async Task<ActionResult<BaseResponse<bool>>> PostProductCategoryType(BaseRequest<CreateProductCategoryTypeModel> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

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
        public async Task<ActionResult<BaseResponse<bool>>> DeleteProductCategoryType(int id)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

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
