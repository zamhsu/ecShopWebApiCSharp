using WebApi.Models.Orders;

namespace WebApi.Base.IServices.Orders
{
    public interface IOrderDetailService
    {
        /// <summary>
        /// 取得訂單商品詳細資料
        /// </summary>
        /// <param name="orderId">訂單編號</param>
        /// <returns></returns>
        Task<List<OrderDetail>> GetAllItemDetailByOrderIdAsync(int orderId);

        /// <summary>
        /// 取得訂單優惠券詳細資料
        /// </summary>
        /// <param name="orderId">訂單編號</param>
        /// <returns></returns>
        Task<OrderDetail?> GetCouponDetailByOrderIdAsync(int orderId);
    }
}