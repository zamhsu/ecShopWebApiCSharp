using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos.Products;
using Service.Interfaces.Products;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Products;
using WebApi.Infrastructures.Models.Paramaters;
using WebApi.Infrastructures.Models.ViewModels;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("api/product")]
        public ActionResult<BaseResponse<ProductGetPagedProductsViewModel>> GetPagedProducts([FromQuery] PageQueryString pageQueryString)
        {
            BaseResponse<ProductGetPagedProductsViewModel> baseResponse = new BaseResponse<ProductGetPagedProductsViewModel>();

            PagedList<ProductDetailDto> products = _productService.GetPagedDetailAllUsable(pageQueryString.PageSize, pageQueryString.Page);
            List<ProductDisplayDto> productDisplays = _mapper.Map<List<ProductDisplayDto>>(products.PagedData);
            Pagination pagination = products.Pagination;

            ProductGetPagedProductsViewModel viewModel = new ProductGetPagedProductsViewModel()
            {
                Products = productDisplays,
                Pagination = pagination
            };

            baseResponse.IsSuccess = true;
            baseResponse.Data = viewModel;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/product/{guid}")]
        public async Task<ActionResult<BaseResponse<ProductDisplayDto>>> GetProduct(string guid)
        {
            BaseResponse<ProductDisplayDto> baseResponse = new BaseResponse<ProductDisplayDto>();

            ProductDetailDto product = await _productService.GetDetailByGuidAsync(guid);
            ProductDisplayDto productDisplay = _mapper.Map<ProductDisplayDto>(product);

            if (product == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = productDisplay;

            return baseResponse;
        }

        [HttpPut("api/admin/product/{guid}")]
        public async Task<ActionResult<BaseResponse<bool>>> PutProduct(string guid, BaseRequest<UpdateProductParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            bool isExists = await _productService.IsExistsAsync(guid);
            if (isExists == false)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            var startOffset = new DateTimeOffset(baseRequest.Data.StartDisplay, baseRequest.UserTimeZone);
            var endOffset = new DateTimeOffset(baseRequest.Data.EndDisplay, baseRequest.UserTimeZone);

            ProductUpdateDto updateDto = _mapper.Map<ProductUpdateDto>(baseRequest.Data);
            updateDto.Guid = guid;
            updateDto.StartDisplay = startOffset;
            updateDto.EndDisplay = endOffset;

            bool result = await _productService.UpdateAsync(updateDto);

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

        [HttpPost("api/admin/product")]
        public async Task<ActionResult<BaseResponse<bool>>> PostProduct(BaseRequest<CreateProductParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            var startOffset = new DateTimeOffset(baseRequest.Data.StartDisplay, baseRequest.UserTimeZone);
            var endOffset = new DateTimeOffset(baseRequest.Data.EndDisplay, baseRequest.UserTimeZone);

            ProductCreateDto createDto = _mapper.Map<ProductCreateDto>(baseRequest.Data);
            createDto.StartDisplay = startOffset;
            createDto.EndDisplay = endOffset;

            bool result = await _productService.CreateAsync(createDto);

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

        [HttpDelete("api/admin/product/{guid}")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteProduct(string guid)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            bool isExists = await _productService.IsExistsAsync(guid);
            if (isExists == false)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            bool result = await _productService.DeleteByGuidAsync(guid);

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
