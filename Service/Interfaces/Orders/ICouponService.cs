using Common.Helpers;
using Repository.Entities.Orders;

namespace Service.Interfaces.Orders
{
    public interface ICouponService
    {
        /// <summary>
        /// 使用id取得一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        Task<Coupon> GetByIdAsync(int id);

        /// <summary>
        /// 使用id取得一筆包含關聯性資料的優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        Task<Coupon> GetDetailByIdAsync(int id);

        /// <summary>
        /// 使用優惠券代碼取得一筆優惠券
        /// </summary>
        /// <param name="code">優惠券代碼</param>
        /// <returns></returns>
        Task<Coupon> GetByCodeAsync(string code);

        /// <summary>
        /// 使用優惠券代碼取得一筆可用的優惠券
        /// </summary>
        /// <param name="code">優惠券代碼</param>
        /// <returns></returns>
        Task<Coupon> GetUsableByCodeAsync(string code);

        /// <summary>
        /// 取得所有優惠券
        /// </summary>
        /// <returns></returns>
        Task<List<Coupon>> GetAllAsync();

        /// <summary>
        /// 取得包含關聯性資料的所有優惠券
        /// </summary>
        /// <returns></returns>
        Task<List<Coupon>> GetDetailAllAsync();

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有優惠券
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        PagedList<Coupon> GetPagedDetailAll(int pageSize, int page);

        /// <summary>
        /// 新增一筆優惠券資料
        /// </summary>
        /// <param name="coupon">新增優惠券的資料</param>
        Task CreateAsync(Coupon coupon);

        /// <summary>
        /// 修改一筆優惠券資料
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <param name="coupon">修改優惠券的資料</param>
        Task UpdateAsync(int id, Coupon coupon);

        /// <summary>
        /// 使用id刪除一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        Task DeleteByIdAsync(int id);
    }
}