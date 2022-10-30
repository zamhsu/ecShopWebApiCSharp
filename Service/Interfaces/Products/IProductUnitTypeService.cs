using Service.Dtos.Products;

namespace Service.Interfaces.Products
{
    public interface IProductUnitTypeService
    {
        /// <summary>
        /// 使用產品單位編號取得一筆產品單位
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <returns></returns>
        Task<ProductUnitTypeDto> GetByIdAsync(int id);

        /// <summary>
        /// 取得所有產品單位
        /// </summary>
        /// <returns></returns>
        Task<List<ProductUnitTypeDto>> GetAllAsync();

        /// <summary>
        /// 新增一筆產品單位資料
        /// </summary>
        /// <param name="createDto">新增產品單位的資料</param>
        /// <returns></returns>
        Task<bool> CreateAsync(ProductUnitTypeCreateDto createDto);

        /// <summary>
        /// 修改一筆產品單位資料
        /// </summary>
        /// <param name="UpdateDto">修改產品單位的資料</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(ProductUnitTypeUpdateDto updateDto);

        /// <summary>
        /// 使用產品單位編號刪除一筆產品單位
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(int id);

        /// <summary>
        /// 產品單位是否存在
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(int id);
    }
}