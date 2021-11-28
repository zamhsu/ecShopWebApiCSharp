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

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
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
        public async Task<ActionResult<BaseResponse<List<ProductDisplayModel>>>> GetProduct()
        {
            BaseResponse<List<ProductDisplayModel>> baseResponse = new BaseResponse<List<ProductDisplayModel>>();

            List<Product> products = await _productService.GetDetailAllUsableAsync();
            List<ProductDisplayModel> productDisplays = _mapper.Map<List<ProductDisplayModel>>(products);

            baseResponse.IsSuccess = true;
            baseResponse.Data = productDisplays;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/product/{guid}")]
        public async Task<ActionResult<BaseResponse<ProductDisplayModel>>> GetProduct(string guid)
        {
            BaseResponse<ProductDisplayModel> baseResponse = new BaseResponse<ProductDisplayModel>();

            Product product = await _productService.GetDetailByGuidAsync(guid);
            ProductDisplayModel productDisplay = _mapper.Map<ProductDisplayModel>(product);

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
        public async Task<ActionResult<BaseResponse<Product>>> PutProduct(string guid, BaseRequest<UpdateProductModel> baseRequest)
        {
            BaseResponse<Product> baseResponse = new BaseResponse<Product>();

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
        public async Task<ActionResult<BaseResponse<Product>>> PostProduct(BaseRequest<CreateProductModel> baseRequest)
        {
            BaseResponse<Product> baseResponse = new BaseResponse<Product>();

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
