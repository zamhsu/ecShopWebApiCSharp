using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.Orders;
using Service.Dtos.Orders;
using Service.Interfaces.Orders;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Orders;
using WebApi.Infrastructures.Models.Paramaters;
using WebApi.Infrastructures.Models.ViewModels;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class CouponController : CustomBaseController
    {
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;

        public CouponController(ICouponService couponService,
            IMapper mapper)
        {
            _couponService = couponService;
            _mapper = mapper;
        }

        [HttpGet("api/admin/coupon")]
        public ActionResult<BaseResponse<CouponGetCouponViewModel>> GetCoupon([FromQuery] PageParameter pageParameter)
        {
            BaseResponse<CouponGetCouponViewModel> baseResponse = new BaseResponse<CouponGetCouponViewModel>();

            PagedList<CouponDetailDto> pagedList = _couponService.GetPagedDetailAll(pageParameter.PageSize, pageParameter.Page);
            List<CouponDisplayDto> couponDisplays = _mapper.Map<List<CouponDisplayDto>>(pagedList.PagedData);
            Pagination pagination = pagedList.Pagination;

            CouponGetCouponViewModel viewModel = new CouponGetCouponViewModel()
            {
                CouponDisplays = couponDisplays,
                Pagination = pagination
            };

            baseResponse.IsSuccess = true;
            baseResponse.Data = viewModel;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/coupon/{id}")]
        public async Task<ActionResult<BaseResponse<CouponDisplayDto>>> GetCoupon(int id)
        {
            BaseResponse<CouponDisplayDto> baseResponse = new BaseResponse<CouponDisplayDto>();

            CouponDetailDto coupon = await _couponService.GetDetailByIdAsync(id);
            CouponDisplayDto couponDisplay = _mapper.Map<CouponDisplayDto>(coupon);

            if (coupon is null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = couponDisplay;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpPost("api/coupon/check")]
        public async Task<ActionResult<BaseResponse<CouponSimpleDto>>> CheckCouponIsUsable(BaseRequest<string> baseRequest)
        {
            BaseResponse<CouponSimpleDto> baseResponse = new BaseResponse<CouponSimpleDto>();

            CouponDto coupon = await _couponService.GetUsableByCodeAsync(baseRequest.Data);

            if (coupon is null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            CouponSimpleDto simpleModel = _mapper.Map<CouponSimpleDto>(coupon);

            baseResponse.IsSuccess = true;
            baseResponse.Data = simpleModel;

            return baseResponse;
        }

        [HttpPut("api/admin/coupon/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> PutCoupon(int id, BaseRequest<UpdateCouponParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            CouponDto existedCoupon = await _couponService.GetByIdAsync(id);
            if (existedCoupon is null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            CouponUpdateDto updateDto = _mapper.Map<CouponUpdateDto>(baseRequest.Data);
            updateDto.Id = id;

            bool result = await _couponService.UpdateAsync(updateDto);

            if (result.Equals(true))
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

        [HttpPost("api/admin/coupon")]
        public async Task<ActionResult<BaseResponse<bool>>> PostCoupon(BaseRequest<CreateCouponParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            var startOffset = new DateTimeOffset(baseRequest.Data.StartDate, baseRequest.UserTimeZone);
            var endOffset = new DateTimeOffset(baseRequest.Data.ExpiredDate, baseRequest.UserTimeZone);

            CouponCreateDto createDto = _mapper.Map<CouponCreateDto>(baseRequest.Data);
            createDto.StartDate = startOffset;
            createDto.ExpiredDate = endOffset;

            bool result = await _couponService.CreateAsync(createDto);

            if(result.Equals(true))
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

        [HttpDelete("api/admin/coupon/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteCoupon(int id)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            CouponDto coupon = await _couponService.GetByIdAsync(id);
            if (coupon is null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            bool result = await _couponService.DeleteByIdAsync(id);

            if(result.Equals(true))
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