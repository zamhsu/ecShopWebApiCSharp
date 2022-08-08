using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.Products;
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

            PagedList<Product> products = _productService.GetPagedDetailAllUsable(pageQueryString.PageSize, pageQueryString.Page);
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

            Product product = await _productService.GetDetailByGuidAsync(guid);
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

            Product existedProduct = await _productService.GetByGuidAsync(guid);
            if (existedProduct == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            var startOffset = new DateTimeOffset(baseRequest.Data.StartDisplay, baseRequest.UserTimeZone);
            var endOffset = new DateTimeOffset(baseRequest.Data.EndDisplay, baseRequest.UserTimeZone);

            Product product = _mapper.Map<Product>(baseRequest.Data);
            product.StartDisplay = startOffset;
            product.EndDisplay = endOffset;

            try
            {
                await _productService.UpdateAsync(guid, product);

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
        public async Task<ActionResult<BaseResponse<bool>>> PostProduct(BaseRequest<CreateProductParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            var startOffset = new DateTimeOffset(baseRequest.Data.StartDisplay, baseRequest.UserTimeZone);
            var endOffset = new DateTimeOffset(baseRequest.Data.EndDisplay, baseRequest.UserTimeZone);

            Product product = _mapper.Map<Product>(baseRequest.Data);
            product.StartDisplay = startOffset;
            product.EndDisplay = endOffset;

            try
            {
                await _productService.CreateAsync(product);

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
        public async Task<ActionResult<BaseResponse<bool>>> DeleteProduct(string guid)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

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
