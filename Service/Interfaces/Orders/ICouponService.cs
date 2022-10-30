using Common.Helpers;
using Service.Dtos.Orders;

namespace Service.Interfaces.Orders
{
    public interface ICouponService
    {
        /// <summary>
        /// 使用id取得一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        Task<CouponDto> GetByIdAsync(int id);

        /// <summary>
        /// 使用id取得一筆包含關聯性資料的優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        Task<CouponDetailDto> GetDetailByIdAsync(int id);

        /// <summary>
        /// 使用優惠券代碼取得一筆優惠券
        /// </summary>
        /// <param name="code">優惠券代碼</param>
        /// <returns></returns>
        Task<CouponDto> GetByCodeAsync(string code);

        /// <summary>
        /// 使用優惠券代碼取得一筆可用的優惠券
        /// </summary>
        /// <param name="code">優惠券代碼</param>
        /// <returns></returns>
        Task<CouponDto> GetUsableByCodeAsync(string code);

        /// <summary>
        /// 取得所有優惠券
        /// </summary>
        /// <returns></returns>
        Task<List<CouponDto>> GetAllAsync();

        /// <summary>
        /// 取得包含關聯性資料的所有優惠券
        /// </summary>
        /// <returns></returns>
        Task<List<CouponDetailDto>> GetDetailAllAsync();

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有優惠券
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        PagedList<CouponDetailDto> GetPagedDetailAll(int pageSize, int page);

        /// <summary>
        /// 新增一筆優惠券資料
        /// </summary>
        /// <param name="createDto">新增優惠券的資料</param>
        /// <returns></returns>
        Task<bool> CreateAsync(CouponCreateDto createDto);

        /// <summary>
        /// 修改一筆優惠券資料
        /// </summary>
        /// <param name="updateDto">修改優惠券的資料</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(CouponUpdateDto updateDto);

        /// <summary>
        /// 使用id刪除一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(int id);
    }
}