using System.Threading;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Dtos.Products;
using WebApi.Models.Products;

namespace WebApi.Base.IServices.Products
{
    public interface IProductService
    {
        /// <summary>
        /// 使用Guid取得一筆產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        Task<Product> GetByGuidAsync(string guid);

        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        Task<Product> GetDetailByGuidAsync(string guid);

        /// <summary>
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAllUsableAsync();

        /// <summary>
        /// 取得包含關聯性資料的所有產品
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetDetailAllUsableAsync();

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有產品
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        PagedList<Product> GetPagedDetailAllUsable(int pageSize, int page);

        /// <summary>
        /// 新增一筆產品資料
        /// </summary>
        /// <param name="product">新增產品的資料</param>
        Task CreateAsync(Product product);

        /// <summary>
        /// 修改一筆產品資料
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <param name="product">修改產品的資料</param>
        Task UpdateAsync(string guid, Product product);

        /// <summary>
        /// 使用Guid刪除一筆產品
        /// </summary>
        /// <param name="guid"></param>
        Task DeleteByGuidAsync(string guid);
    }
}