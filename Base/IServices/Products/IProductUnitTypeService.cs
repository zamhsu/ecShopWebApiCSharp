using WebApi.Dtos.Products;
using WebApi.Models.Products;

namespace WebApi.Base.IServices.Products
{
    public interface IProductUnitTypeService
    {
        /// <summary>
        /// 使用產品單位編號取得一筆產品單位
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <returns></returns>
        Task<ProductUnitType> GetByIdAsync(int id);

        /// <summary>
        /// 取得所有產品單位
        /// </summary>
        /// <returns></returns>
        List<ProductUnitType> GetAll();

        /// <summary>
        /// 新增一筆產品單位資料
        /// </summary>
        /// <param name="createProductUnitType">新增產品單位的資料</param>
        Task CreateAsync(CreateProductUnitTypeModel createProductUnitType);

        /// <summary>
        /// 修改一筆產品單位資料
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <param name="updateProductUnitType">修改產品單位的資料</param>
        Task UpdateAsync(int id, UpdateProductUnitTypeModel updateProductUnitType);

        /// <summary>
        /// 使用產品單位編號刪除一筆產品單位
        /// </summary>
        /// <param name="id">產品單位編號</param>
        Task DeleteByIdAsync(int id);
    }
}