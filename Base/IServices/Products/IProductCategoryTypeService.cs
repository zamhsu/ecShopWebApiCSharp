using WebApi.Models.Products;

namespace WebApi.Base.IServices.Products
{
    public interface IProductCategoryTypeService
    {
        /// <summary>
        /// 使用產品分類編號取得一筆產品分類
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <returns></returns>
        Task<ProductCategoryType> GetByIdAsync(int id);

        /// <summary>
        /// 取得所有產品分類
        /// </summary>
        /// <returns></returns>
        List<ProductCategoryType> GetAll();

        /// <summary>
        /// 新增一筆產品分類資料
        /// </summary>
        /// <param name="productCategoryType">新增產品分類的資料</param>
        Task CreateAsync(ProductCategoryType productCategoryType);

        /// <summary>
        /// 修改一筆產品分類資料
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <param name="productCategoryType">修改產品分類的資料</param>
        Task UpdateAsync(int id, ProductCategoryType productCategoryType);
    }
}