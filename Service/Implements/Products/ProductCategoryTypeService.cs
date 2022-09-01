using AutoMapper;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Products;
using Repository.Interfaces;
using Service.Interfaces.Products;

namespace Service.Implements.Products
{
    public class ProductCategoryTypeService : IProductCategoryTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<ProductCategoryType> _logger;

        public ProductCategoryTypeService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppLogger<ProductCategoryType> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用產品分類編號取得一筆產品分類
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <returns></returns>
        public async Task<ProductCategoryType> GetByIdAsync(int id)
        {
            ProductCategoryType productCategoryType = await _unitOfWork.Repository<ProductCategoryType>()
                .GetAsync(q => q.Id == id);

            return productCategoryType;
        }

        /// <summary>
        /// 取得所有產品分類
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductCategoryType>> GetAllAsync()
        {
            IQueryable<ProductCategoryType> query = _unitOfWork.Repository<ProductCategoryType>().GetAll();
            List<ProductCategoryType> productCategoryTypes = await query.ToListAsync();

            return productCategoryTypes;
        }

        /// <summary>
        /// 新增一筆產品分類資料
        /// </summary>
        /// <param name="productCategoryType">新增產品分類的資料</param>
        public async Task CreateAsync(ProductCategoryType productCategoryType)
        {
            if (productCategoryType == null)
            {
                _logger.LogInformation("[Create] ProductCategoryType can not be null");
                throw new ArgumentNullException(nameof(productCategoryType));
            }

            await _unitOfWork.Repository<ProductCategoryType>().CreateAsync(productCategoryType);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改一筆產品分類資料
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <param name="productCategoryType">修改產品分類的資料</param>
        public async Task UpdateAsync(int id, ProductCategoryType productCategoryType)
        {
            ProductCategoryType entity = await GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] ProductCategoryType is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(productCategoryType));
            }

            entity.Name = productCategoryType.Name;

            _unitOfWork.Repository<ProductCategoryType>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 使用產品分類編號刪除一筆產品分類
        /// </summary>
        /// <param name="id">產品分類編號</param>
        public async Task DeleteByIdAsync(int id)
        {
            ProductCategoryType entity = await GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] ProductCategoryType is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Deleted = true;

            _unitOfWork.Repository<ProductCategoryType>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}