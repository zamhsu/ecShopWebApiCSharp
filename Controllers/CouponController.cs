using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Base.IServices.Orders;
using WebApi.Dtos;
using WebApi.Dtos.Orders;
using WebApi.Dtos.ViewModel;
using WebApi.Models.Orders;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class CouponController : ControllerBase
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
        public ActionResult<BaseResponse<CouponGetCouponViewModel>> GetCoupon([FromQuery] PageQueryString pageQueryString)
        {
            BaseResponse<CouponGetCouponViewModel> baseResponse = new BaseResponse<CouponGetCouponViewModel>();

            PagedList<Coupon> pagedList = _couponService.GetPagedDetailAll(pageQueryString.PageSize, pageQueryString.Page);
            List<CouponDisplayModel> couponDisplays = _mapper.Map<List<CouponDisplayModel>>(pagedList.PagedData);
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
        public async Task<ActionResult<BaseResponse<CouponDisplayModel>>> GetCoupon(int id)
        {
            BaseResponse<CouponDisplayModel> baseResponse = new BaseResponse<CouponDisplayModel>();

            Coupon? coupon = await _couponService.GetDetailByIdAsync(id);
            CouponDisplayModel couponDisplay = _mapper.Map<CouponDisplayModel>(coupon);

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

        [HttpPut("api/admin/coupon/{id}")]
        public async Task<ActionResult<BaseResponse<Coupon>>> PutCoupon(int id, BaseRequest<UpdateCouponModel> baseRequest)
        {
            BaseResponse<Coupon> baseResponse = new BaseResponse<Coupon>();

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
        public async Task<ActionResult<BaseResponse<Coupon>>> PostCoupon(BaseRequest<CreateCouponModel> baseRequest)
        {
            BaseResponse<Coupon> baseResponse = new BaseResponse<Coupon>();

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
        public async Task<ActionResult<BaseResponse<Coupon>>> DeleteCoupon(int id)
        {
            BaseResponse<Coupon> baseResponse = new BaseResponse<Coupon>();

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