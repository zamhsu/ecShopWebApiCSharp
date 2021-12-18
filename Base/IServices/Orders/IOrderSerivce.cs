using WebApi.Dtos.Orders;
using WebApi.Models.Orders;
using WebApi.Models.Products;

namespace WebApi.Base.IServices.Orders
{
    public interface IOrderService
    {
        /// <summary>
        /// 使用Guid取得一筆訂單
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        Task<Order> GetByGuidAsync(string guid);

        /// <summary>
        /// 使用Guid取得一筆訂單詳細資料
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        Task<Order?> GetDetailByGuidAsync(string guid);

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetAllAsync();

        /// <summary>
        /// 取得所有訂單詳細資料
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetDetailAllAsync();

        /// <summary>
        /// 下訂單
        /// </summary>
        /// <param name="order">訂單資料</param>
        /// <param name="placeOrderDetails">商品</param>
        /// <param name="coupon">優惠券</param>
        /// <returns></returns>
        Task PlaceOrderAsync(Order order, List<PlaceOrderDetailModel> placeOrderDetails, Coupon? coupon);

        /// <summary>
        /// 修改一筆訂單資料
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="order">修改訂單的資料</param>
        Task UpdateAsync(string guid, Order order);
    }
}