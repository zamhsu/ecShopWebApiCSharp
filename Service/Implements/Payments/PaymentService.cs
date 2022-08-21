using Repository.Entities.Orders;
using Repository.Interfaces;
using Service.Dtos.Orders;
using Service.Interfaces.Orders;
using Service.Interfaces.Payments;

namespace Service.Implments.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;

        public PaymentService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// 使用信用卡付款
        /// </summary>
        /// <param name="orderGuid">訂單GUID</param>
        /// <returns></returns>
        public async Task<bool> PayWithCreditCardAsync(string orderGuid)
        {
            OrderDto order = await _orderService.GetByGuidAsync(orderGuid);

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
            OrderDto order = await _orderService.GetByGuidAsync(orderGuid);

            if (order == null)
            {
                return false;
            }

            return true;
        }
    }
}