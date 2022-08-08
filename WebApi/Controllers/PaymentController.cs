using AutoMapper;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.Orders;
using Service.Interfaces.Orders;
using Service.Interfaces.Payments;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Payments;
using WebApi.Infrastructures.Models.Paramaters;
using WebApi.Infrastructures.Models.ViewModels;

namespace WebApi.Controllers
{
    [ApiController]
    public class PaymentController : CustomBaseController
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
        public async Task<ActionResult<BaseResponse<List<PaymentMethodDisplayDto>>>> GetPaymentMethod()
        {
            List<PaymentMethod> methodList = await _paymentMethodService.GetAllAsync();
            List<PaymentMethodDisplayDto> displayList = _mapper.Map<List<PaymentMethodDisplayDto>>(methodList);
            BaseResponse<List<PaymentMethodDisplayDto>> baseResponse = new BaseResponse<List<PaymentMethodDisplayDto>>()
            {
                IsSuccess = true,
                Data = displayList
            };

            return baseResponse;
        }

        [HttpPost("api/payment/creditCard")]
        public async Task<ActionResult<BaseResponse<bool>>> PayWithCreditCard(CustomerPaymentParameter parameter)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();
            PaymentMethodEnum paymentMethodEnum = PaymentMethodEnum.CreditCard;

            bool isAllowedMethod = await _paymentMethodService.IsAllowedMethodAsync(parameter.PaymentMethodId);
            bool isCreditCard = parameter.PaymentMethodId == (int)paymentMethodEnum;
            if (!isAllowedMethod || !isCreditCard)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "無效的付款方式";

                return baseResponse;
            }

            bool isPaid = await _orderService.IsOrderPaidAsync(parameter.OrderGuid);
            if (isPaid)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "此訂單已付款";

                return baseResponse;
            }

            try
            {
                bool paySuccessful = await _paymentService.PayWithCreditCardAsync(parameter.OrderGuid);
                if (!paySuccessful)
                {
                    await _orderService.UpdateStatusToPaymentFailedAsync(parameter.OrderGuid, paymentMethodEnum);

                    baseResponse.IsSuccess = false;
                    baseResponse.Message = "付款失敗";

                    return baseResponse;
                }

                await _orderService.UpdateStatusToPaymentSuccessfulAsync(parameter.OrderGuid, paymentMethodEnum);

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
        public async Task<ActionResult<BaseResponse<bool>>> PayWithAtm(CustomerPaymentParameter parameter)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();
            PaymentMethodEnum paymentMethodEnum = PaymentMethodEnum.ATM;

            bool isAllowedMethod = await _paymentMethodService.IsAllowedMethodAsync(parameter.PaymentMethodId);
            bool isCreditCard = parameter.PaymentMethodId == (int)paymentMethodEnum;
            if (!isAllowedMethod || !isCreditCard)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "無效的付款方式";

                return baseResponse;
            }

            bool isPaid = await _orderService.IsOrderPaidAsync(parameter.OrderGuid);
            if (isPaid)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "此訂單已付款";

                return baseResponse;
            }

            try
            {
                bool paySuccessful = await _paymentService.PayWithCreditCardAsync(parameter.OrderGuid);
                if (!paySuccessful)
                {
                    await _orderService.UpdateStatusToPaymentFailedAsync(parameter.OrderGuid, paymentMethodEnum);

                    baseResponse.IsSuccess = false;
                    baseResponse.Message = "付款失敗";

                    return baseResponse;
                }

                await _orderService.UpdateStatusToPaymentSuccessfulAsync(parameter.OrderGuid, paymentMethodEnum);

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