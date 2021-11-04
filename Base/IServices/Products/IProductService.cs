using System.Threading;
using System.Threading.Tasks;
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
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        List<Product> GetAllUsable();

        /// <summary>
        /// 新增一筆產品資料
        /// </summary>
        /// <param name="createProductModel">新增產品的資料</param>
        /// <param name="userTimeZone">使用者時區</param>
        Task CreateAsync(CreateProductModel createProductModel, TimeSpan userTimeZone);

        /// <summary>
        /// 修改一筆產品資料
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <param name="updateProductModel">修改產品的資料</param>
        /// <param name="userTimeZone">使用者時區</param>
        Task UpdateAsync(string guid, UpdateProductModel updateProductModel, TimeSpan userTimeZone);

        /// <summary>
        /// 使用Guid刪除一筆產品
        /// </summary>
        /// <param name="guid"></param>
        Task DeleteByGuidAsync(string guid);
    }
}