using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.Orders;
using Service.Interfaces.Orders;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Orders;
using WebApi.Infrastructures.Models.InputParamaters;
using WebApi.Infrastructures.Models.OutputModels;

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

            PagedList<Coupon> pagedList = _couponService.GetPagedDetailAll(pageParameter.PageSize, pageParameter.Page);
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

            Coupon coupon = await _couponService.GetDetailByIdAsync(id);
            CouponDisplayDto couponDisplay = _mapper.Map<CouponDisplayDto>(coupon);

            if (coupon == null)
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

            Coupon coupon = await _couponService.GetUsableByCodeAsync(baseRequest.Data);

            if (coupon == null)
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

            Coupon existedCoupon = await _couponService.GetByIdAsync(id);
            if (existedCoupon == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            Coupon coupon = _mapper.Map<Coupon>(baseRequest.Data);

            try
            {
                await _couponService.UpdateAsync(id, coupon);

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

        [HttpPost("api/admin/coupon")]
        public async Task<ActionResult<BaseResponse<bool>>> PostCoupon(BaseRequest<CreateCouponParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            var startOffset = new DateTimeOffset(baseRequest.Data.StartDate, baseRequest.UserTimeZone);
            var endOffset = new DateTimeOffset(baseRequest.Data.ExpiredDate, baseRequest.UserTimeZone);

            Coupon coupon = _mapper.Map<Coupon>(baseRequest.Data);
            coupon.StartDate = startOffset;
            coupon.ExpiredDate = endOffset;

            try
            {
                await _couponService.CreateAsync(coupon);

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

        [HttpDelete("api/admin/coupon/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteCoupon(int id)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            Coupon coupon = await _couponService.GetByIdAsync(id);
            if (coupon == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _couponService.DeleteByIdAsync(id);

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