using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Orders;
using WebApi.Base.IServices.Payments;
using WebApi.Models;
using WebApi.Models.Orders;

namespace WebApi.Base.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IOrderService _orderService;

        public PaymentService(IRepository<PaymentMethod> paymentMethodRepository,
            IOrderService orderService)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _orderService = orderService;
        }

        /// <summary>
        /// 使用信用卡付款
        /// </summary>
        /// <param name="orderGuid">訂單GUID</param>
        /// <returns></returns>
        public async Task<bool> PayWithCreditCardAsync(string orderGuid)
        {
            Order order = await _orderService.GetByGuidAsync(orderGuid);

            if (order == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 使用ATM轉帳
        /// </summary>
        /// <param name="orderGuid">訂單GUID</param>
        /// <returns></returns>
        public async Task<bool> PayWithAtmAsync(string orderGuid)
        {
            Order order = await _orderService.GetByGuidAsync(orderGuid);

            if (order == null)
            {
                return false;
            }

            return true;
        }
    }
}