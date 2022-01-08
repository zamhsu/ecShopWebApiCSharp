using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Base.IServices.Orders;
using WebApi.Base.IServices.Payments;
using WebApi.Dtos;
using WebApi.Dtos.Payments;
using WebApi.Models;
using WebApi.Models.Orders;

namespace WebApi.Controllers
{
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IOrderService orderService,
            IPaymentMethodService paymentMethodService,
            IPaymentService paymentService,
            IMapper mapper)
        {
            _orderService = orderService;
            _paymentMethodService = paymentMethodService;
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpGet("api/payment/method")]
        public async Task<ActionResult<BaseResponse<List<PaymentMethodDisplayModel>>>> GetPaymentMethod()
        {
            List<PaymentMethod> methodList = await _paymentMethodService.GetAllAsync();
            List<PaymentMethodDisplayModel> displayList = _mapper.Map<List<PaymentMethodDisplayModel>>(methodList);
            BaseResponse<List<PaymentMethodDisplayModel>> baseResponse = new BaseResponse<List<PaymentMethodDisplayModel>>()
            {
                IsSuccess = true,
                Data = displayList
            };

            return baseResponse;
        }

        [HttpPost("api/payment/creditCard")]
        public async Task<ActionResult<BaseResponse<bool>>> PayWithCreditCard(CustomerPaymentModel model)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();
            PaymentMethodPara paymentMethodPara = PaymentMethodPara.CreditCard;

            bool isAllowedMethod = await _paymentMethodService.IsAllowedMethodAsync(model.PaymentMethodId);
            bool isCreditCard = model.PaymentMethodId == (int)paymentMethodPara;
            if (!isAllowedMethod || !isCreditCard)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "無效的付款方式";

                return baseResponse;
            }

            bool isPaid = await _orderService.IsOrderPaidAsync(model.OrderGuid);
            if (isPaid)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "此訂單已付款";

                return baseResponse;
            }

            try
            {
                bool paySuccessful = await _paymentService.PayWithCreditCardAsync(model.OrderGuid);
                if (!paySuccessful)
                {
                    await _orderService.UpdateStatusToPaymentFailedAsync(model.OrderGuid, paymentMethodPara);

                    baseResponse.IsSuccess = false;
                    baseResponse.Message = "付款失敗";

                    return baseResponse;
                }

                await _orderService.UpdateStatusToPaymentSuccessfulAsync(model.OrderGuid, paymentMethodPara);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "付款完成";

                return baseResponse;
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "訂單狀態修改失敗";

                return baseResponse;
            }
        }

        [HttpPost("api/payment/atm")]
        public async Task<ActionResult<BaseResponse<bool>>> PayWithAtm(CustomerPaymentModel model)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();
            PaymentMethodPara paymentMethodPara = PaymentMethodPara.ATM;

            bool isAllowedMethod = await _paymentMethodService.IsAllowedMethodAsync(model.PaymentMethodId);
            bool isCreditCard = model.PaymentMethodId == (int)paymentMethodPara;
            if (!isAllowedMethod || !isCreditCard)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "無效的付款方式";

                return baseResponse;
            }

            bool isPaid = await _orderService.IsOrderPaidAsync(model.OrderGuid);
            if (isPaid)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "此訂單已付款";

                return baseResponse;
            }

            try
            {
                bool paySuccessful = await _paymentService.PayWithCreditCardAsync(model.OrderGuid);
                if (!paySuccessful)
                {
                    await _orderService.UpdateStatusToPaymentFailedAsync(model.OrderGuid, paymentMethodPara);

                    baseResponse.IsSuccess = false;
                    baseResponse.Message = "付款失敗";

                    return baseResponse;
                }

                await _orderService.UpdateStatusToPaymentSuccessfulAsync(model.OrderGuid, paymentMethodPara);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "付款完成";

                return baseResponse;
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "訂單狀態修改失敗";

                return baseResponse;
            }
        }
    }
}