using WebApi.Models.Orders;

namespace WebApi.Base.IServices.Payments
{
    public interface IPaymentMethodService
    {
        /// <summary>
        /// 取得一筆付款方式
        /// </summary>
        /// <param name="id">付款方式Id</param>
        /// <returns></returns>
        Task<PaymentMethod?> GetByIdAsync(int id);

        /// <summary>
        /// 取得所有付款方式
        /// </summary>
        /// <returns></returns>
        Task<List<PaymentMethod>> GetAllAsync();

        /// <summary>
        /// 付款方式是否為允許使用
        /// </summary>
        /// <param name="id">付款方式Id</param>
        /// <returns></returns>
        Task<bool> IsAllowedMethodAsync(int id);
    }
}