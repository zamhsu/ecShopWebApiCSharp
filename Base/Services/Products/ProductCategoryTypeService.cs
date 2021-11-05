using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Products;
using WebApi.Models;
using WebApi.Models.Products;

namespace WebApi.Base.Services.Products
{
    public class ProductCategoryTypeService : IProductCategoryTypeService
    {
        private readonly IRepository<ProductCategoryType> _productCategoryTypeRepository;
        private readonly IAppLogger<ProductCategoryType> _logger;

        public ProductCategoryTypeService(IRepository<ProductCategoryType> productCategoryTypeRepository,
            IAppLogger<ProductCategoryType> logger)
        {
            _productCategoryTypeRepository = productCategoryTypeRepository;
            _logger = logger;
        }

        /// <summary>
        /// 使用產品分類編號取得一筆產品分類
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <returns></returns>
        public async Task<ProductCategoryType> GetByIdAsync(int id)
        {
            ProductCategoryType productCategoryType = await _productCategoryTypeRepository.GetAsync(q => q.Id == id);

            return productCategoryType;
        }

        /// <summary>
        /// 取得所有產品分類
        /// </summary>
        /// <returns></returns>
        public List<ProductCategoryType> GetAll()
        {
            IQueryable<ProductCategoryType> query = _productCategoryTypeRepository.GetAll();
            List<ProductCategoryType> productCategoryTypes = query.ToList();

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
                new ArgumentNullException(nameof(productCategoryType));
            }

            try
            {
                await _productCategoryTypeRepository.CreateAsync(productCategoryType);
            }
            catch
            {
                throw;
            }
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

            try
            {
                await _productCategoryTypeRepository.UpdateAsync(entity);
            }
            catch
            {
                throw;
            }
        }
    }
}