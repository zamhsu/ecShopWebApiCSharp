using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.Orders;
using Service.Dtos.Orders;
using Service.Interfaces.Orders;
using Service.Interfaces.Products;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Orders;
using WebApi.Infrastructures.Models.InputParamaters;
using WebApi.Infrastructures.Models.OutputModels;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class OrderController : CustomBaseController
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
        public ActionResult<BaseResponse<OrderGetOrderViewModel>> GetOrder([FromQuery] PageParameter pageParameter)
        {
            BaseResponse<OrderGetOrderViewModel> baseResponse = new BaseResponse<OrderGetOrderViewModel>();

            PagedList<Order> pagedList = _orderService.GetPagedDetailAll(pageParameter.PageSize, pageParameter.Page);
            List<OrderDisplayDto> orderDisplays = _mapper.Map<List<OrderDisplayDto>>(pagedList.PagedData);
            Pagination pagination = pagedList.Pagination;

            OrderGetOrderViewModel viewModel = new OrderGetOrderViewModel()
            {
                OrderDisplays = orderDisplays,
                Pagination = pagination
            };

            baseResponse.IsSuccess = true;
            baseResponse.Data = viewModel;

            return baseResponse;
        }

        [HttpGet("api/admin/order/{guid}")]
        public async Task<ActionResult<BaseResponse<OrderDisplayDetailDto>>> GetOrder(string guid)
        {
            BaseResponse<OrderDisplayDetailDto> baseResponse = new BaseResponse<OrderDisplayDetailDto>();

            OrderDisplayDetailDto orderDisplayDto = await _orderService.GetDetailByGuidAsync(guid);

            if (orderDisplayDto == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = orderDisplayDto;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpPost("api/order/customerInfo")]
        public async Task<ActionResult<BaseResponse<OrderGetCustomerOrdersViewModel>>> GetCustomerOrders([FromQuery] PageParameter pageParameter, [FromBody] BaseRequest<CustomerOrderQueryParameter> baseRequest)
        {
            PagedList<OrderDisplayDetailDto> pagedList = await _orderService.GetPagedDetailByCustomerInfoAsync(pageParameter.PageSize, pageParameter.Page, baseRequest.Data.Name, baseRequest.Data.Email, baseRequest.Data.Phone);
            
            OrderGetCustomerOrdersViewModel viewModel = new OrderGetCustomerOrdersViewModel()
            {
                OrderDisplays = pagedList.PagedData,
                Pagination = pagedList.Pagination
            };

            BaseResponse<OrderGetCustomerOrdersViewModel> baseResponse = new BaseResponse<OrderGetCustomerOrdersViewModel>();
            baseResponse.IsSuccess = true;
            baseResponse.Data = viewModel;

            return baseResponse;
        }

        [AllowAnonymous]
        [HttpPut("api/order/customerInfo/{guid}")]
        public async Task<ActionResult<BaseResponse<bool>>> PutOrderCustomerInfo(string guid, BaseRequest<UpdateOrderCustomerInfoParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

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
        public async Task<ActionResult<BaseResponse<string>>> PlaceOrder(BaseRequest<PlaceOrderParameter> baseRequest)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            if (string.IsNullOrWhiteSpace(baseRequest.Data.CouponCode))
            {
                baseRequest.Data.CouponCode = "";
            }

            Coupon coupon = await _couponService.GetUsableByCodeAsync(baseRequest.Data.CouponCode);

            Order order = _mapper.Map<Order>(baseRequest.Data.Order);

            try
            {
                string guid = await _orderService.PlaceOrderAsync(order, baseRequest.Data.OrderDetailModels, coupon);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "建立成功";
                baseResponse.Data = guid;
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