using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Base.IServices.Orders;
using WebApi.Base.IServices.Products;
using WebApi.Dtos;
using WebApi.Dtos.Orders;
using WebApi.Dtos.ViewModel;
using WebApi.Models.Orders;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICouponService _couponService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderSerivce,
            ICouponService couponService,
            IProductService productService,
            IMapper mapper)
        {
            _orderService = orderSerivce;
            _couponService = couponService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("api/admin/order")]
        public async Task<ActionResult<BaseResponse<List<OrderDisplayModel>>>> GetOrder()
        {
            BaseResponse<List<OrderDisplayModel>> baseResponse = new BaseResponse<List<OrderDisplayModel>>();

            List<Order> orders = await _orderService.GetDetailAllAsync();
            List<OrderDisplayModel> orderDisplays = _mapper.Map<List<OrderDisplayModel>>(orders);

            baseResponse.IsSuccess = true;
            baseResponse.Data = orderDisplays;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpGet("api/order/{guid}")]
        public async Task<ActionResult<BaseResponse<OrderDisplayModel>>> GetOrder(string guid)
        {
            BaseResponse<OrderDisplayModel> baseResponse = new BaseResponse<OrderDisplayModel>();

            OrderDisplayModel? orderDisplayModel = await _orderService.GetDetailByGuidAsync(guid);

            if (orderDisplayModel == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = orderDisplayModel;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpPut("api/order/customerInfo/{guid}")]
        public async Task<ActionResult<BaseResponse<Order>>> PutOrderCustomerInfo(string guid, BaseRequest<UpdateOrderCustomerInfoModel> baseRequest)
        {
            BaseResponse<Order> baseResponse = new BaseResponse<Order>();

            Order existedOrder = await _orderService.GetByGuidAsync(guid);
            if (existedOrder == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            Order order = _mapper.Map<Order>(baseRequest.Data);

            try
            {
                await _orderService.UpdateCustomerInfoAsync(guid, order);

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

        [AllowAnonymous]
        [HttpPost("api/order/place")]
        public async Task<ActionResult<BaseResponse<Order>>> PlaceOrder(BaseRequest<OrderPlaceOrderViewModel> baseRequest)
        {
            BaseResponse<Order> baseResponse = new BaseResponse<Order>();

            Coupon coupon = await _couponService.GetByCodeAsync(baseRequest.Data.CouponCode);
            Order order = _mapper.Map<Order>(baseRequest.Data.Order);

            try
            {
                await _orderService.PlaceOrderAsync(order, baseRequest.Data.OrderDetailModels, coupon);

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
    }
}