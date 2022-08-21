using Service.Dtos.Orders;

namespace Service.Interfaces.Orders
{
    public interface IOrderDetailService
    {
        /// <summary>
        /// 取得訂單商品詳細資料
        /// </summary>
        /// <param name="orderId">訂單編號</param>
        /// <returns></returns>
        Task<List<OrderItemDetailDisplayDto>> GetAllItemDetailByOrderIdAsync(int orderId);

        /// <summary>
        /// 取得訂單優惠券詳細資料
        /// </summary>
        /// <param name="orderId">訂單編號</param>
        /// <returns></returns>
        Task<OrderCouponDetailDisplayDto> GetCouponDetailByOrderIdAsync(int orderId);
    }
}