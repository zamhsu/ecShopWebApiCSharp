using AutoMapper;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Products;
using Repository.Interfaces;
using Service.Interfaces.Products;

namespace Service.Implements.Products
{
    public class ProductUnitTypeService : IProductUnitTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<ProductUnitType> _logger;

        public ProductUnitTypeService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppLogger<ProductUnitType> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用產品單位編號取得一筆產品單位
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <returns></returns>
        public async Task<ProductUnitType> GetByIdAsync(int id)
        {
            ProductUnitType productUnitType = await _unitOfWork.Repository<ProductUnitType>()
                .GetAsync(q => q.Id == id);

            return productUnitType;
        }

        /// <summary>
        /// 取得所有產品單位
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductUnitType>> GetAllAsync()
        {
            IQueryable<ProductUnitType> query = _unitOfWork.Repository<ProductUnitType>().GetAll();
            List<ProductUnitType> productUnitTypes = await query.ToListAsync();

            return productUnitTypes;
        }

        /// <summary>
        /// 新增一筆產品單位資料
        /// </summary>
        /// <param name="productUnitType">新增產品單位的資料</param>
        public async Task CreateAsync(ProductUnitType productUnitType)
        {
            if (productUnitType == null)
            {
                _logger.LogInformation("[Create] ProductUnitType can not be null");
                throw new ArgumentNullException(nameof(productUnitType));
            }

            await _unitOfWork.Repository<ProductUnitType>().CreateAsync(productUnitType);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改一筆產品單位資料
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <param name="productUnitType">修改產品單位的資料</param>
        public async Task UpdateAsync(int id, ProductUnitType updateProductUnitType)
        {
            ProductUnitType entity = await GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] ProductUnitType is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(updateProductUnitType));
            }

            entity.Name = updateProductUnitType.Name;

            _unitOfWork.Repository<ProductUnitType>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 使用產品單位編號刪除一筆產品單位
        /// </summary>
        /// <param name="id">產品單位編號</param>
        public async Task DeleteByIdAsync(int id)
        {
            ProductUnitType entity = await GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] ProductUnitType is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Deleted = true;

            _unitOfWork.Repository<ProductUnitType>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}