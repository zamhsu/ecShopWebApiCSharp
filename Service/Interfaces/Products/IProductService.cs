using Common.Helpers;
using Service.Dtos.Products;

namespace Service.Interfaces.Products
{
    public interface IProductService
    {
        /// <summary>
        /// 使用Guid取得一筆產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        Task<ProductDto> GetByGuidAsync(string guid);

        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        Task<ProductDetailDto> GetDetailByGuidAsync(string guid);

        /// <summary>
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        Task<List<ProductDto>> GetAllUsableAsync();

        /// <summary>
        /// 取得包含關聯性資料的所有產品
        /// </summary>
        /// <returns></returns>
        Task<List<ProductDetailDto>> GetDetailAllUsableAsync();

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有產品
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        PagedList<ProductDetailDto> GetPagedDetailAllUsable(int pageSize, int page);

        /// <summary>
        /// 新增一筆產品資料
        /// </summary>
        /// <param name="createDto">新增產品的資料</param>
        Task<bool> CreateAsync(ProductCreateDto createDto);

        /// <summary>
        /// 修改一筆產品資料
        /// </summary>
        /// <param name="updateDto">修改產品的資料</param>
        Task<bool> UpdateAsync(ProductUpdateDto updateDto);

        /// <summary>
        /// 使用Guid刪除一筆產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        Task<bool> DeleteByGuidAsync(string guid);

        /// <summary>
        /// 產品是否存在
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(string guid);
    }
}