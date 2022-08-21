using Common.Enums;
using Common.Helpers;
using Repository.Entities.Orders;
using Service.Dtos.Orders;

namespace Service.Interfaces.Orders
{
    public interface IOrderService
    {
        /// <summary>
        /// 使用Guid取得一筆訂單
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        Task<OrderDto> GetByGuidAsync(string guid);

        /// <summary>
        /// 使用Guid取得一筆訂單詳細資料
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        Task<OrderDisplayDetailDto> GetDetailByGuidAsync(string guid);

        /// <summary>
        /// 使用消費者資料取得分頁後的訂單詳細資料
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <param name="name">姓名</param>
        /// <param name="email">Email</param>
        /// <param name="phone">聯絡電話</param>
        /// <returns></returns>
        Task<PagedList<OrderDisplayDetailDto>> GetPagedDetailByCustomerInfoAsync(int pageSize, int page, string name, string email, string phone);

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns></returns>
        Task<List<OrderDto>> GetAllAsync();

        /// <summary>
        /// 取得所有訂單詳細資料
        /// </summary>
        /// <returns></returns>
        Task<List<OrderDetailDto>> GetDetailAllAsync();

        /// <summary>
        /// 取得分頁後所有訂單詳細資料
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        PagedList<OrderDetailDto> GetPagedDetailAll(int pageSize, int page);

        /// <summary>
        /// 下訂單
        /// </summary>
        /// <param name="customerInfoDto">顧客資料</param>
        /// <param name="placeOrderDetails">商品</param>
        /// <param name="couponCode">優惠券代碼</param>
        /// <returns></returns>
        Task<string> PlaceOrderAsync(OrderCustomerInfoDto customerInfoDto, List<PlaceOrderDetailDto> placeOrderDetails, string couponCode);

        /// <summary>
        /// 修改一筆訂單裡的消費者個人資料
        /// </summary>
        /// <param name="customerInfoDto">修改訂單的資料</param>
        /// <returns></returns>
        Task<bool> UpdateCustomerInfoAsync(OrderCustomerInfoDto customerInfoDto);

        /// <summary>
        /// 修改一筆訂單付款狀態
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="paymentMethod">付款方式</param>
        /// <param name="orderStatus">訂單狀態</param>
        /// <returns></returns>
        Task<bool> UpdatePaymentStatusAsync(string guid, PaymentMethodEnum paymentMethod, OrderStatusEnum orderStatus);

        /// <summary>
        /// 修改一筆訂單資料
        /// </summary>
        /// <param name="updateDto">修改訂單的資料</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(OrderUpdateDto updateDto);

        /// <summary>
        /// 訂單是否已經付款
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        Task<bool> IsOrderPaidAsync(string guid);

        /// <summary>
        /// 訂單是否存在
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(string guid);
    }
}