using WebApi.Models.Orders;

namespace WebApi.Base.IServices.Payments
{
    public interface IPaymentService
    {
        /// <summary>
        /// 使用信用卡付款
        /// </summary>
        /// <param name="orderGuid">訂單GUID</param>
        /// <returns></returns>
        Task<bool> PayWithCreditCardAsync(string orderGuid);

        /// <summary>
        /// 使用ATM轉帳
        /// </summary>
        /// <param name="orderGuid">訂單GUID</param>
        /// <returns></returns>
        Task<bool> PayWithAtmAsync(string orderGuid);
    }
}