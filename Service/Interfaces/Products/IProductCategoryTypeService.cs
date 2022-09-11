using Repository.Entities.Products;
using Service.Dtos.Products;

namespace Service.Interfaces.Products
{
    public interface IProductCategoryTypeService
    {
        /// <summary>
        /// 使用產品分類編號取得一筆產品分類
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <returns></returns>
        Task<ProductCategoryTypeDto> GetByIdAsync(int id);

        /// <summary>
        /// 取得所有產品分類
        /// </summary>
        /// <returns></returns>
        Task<List<ProductCategoryTypeDto>> GetAllAsync();

        /// <summary>
        /// 新增一筆產品分類資料
        /// </summary>
        /// <param name="createDto">新增產品分類的資料</param>
        Task<bool> CreateAsync(ProductCategoryTypeCreateDto createDto);

        /// <summary>
        /// 修改一筆產品分類資料
        /// </summary>
        /// <param name="updateDto">修改產品分類的資料</param>
        Task<bool> UpdateAsync(ProductCategoryTypeUpdateDto updateDto);

        /// <summary>
        /// 使用產品分類編號刪除一筆產品分類
        /// </summary>
        /// <param name="id">產品分類編號</param>
        Task<bool> DeleteByIdAsync(int id);

        /// <summary>
        /// 指定產品分類是否存在
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(int id);
    }
}