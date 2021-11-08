using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Products;
using WebApi.Dtos.Products;
using WebApi.Models;
using WebApi.Models.Products;

namespace WebApi.Base.Services.Products
{
    public class ProductUnitTypeService : IProductUnitTypeService
    {
        private readonly IRepository<ProductUnitType> _productUnitTypeRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<ProductUnitType> _logger;

        public ProductUnitTypeService(IRepository<ProductUnitType> productUnitTypeRepository,
            IMapper mapper,
            IAppLogger<ProductUnitType> logger)
        {
            _productUnitTypeRepository = productUnitTypeRepository;
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
            ProductUnitType productUnitType = await _productUnitTypeRepository.GetAsync(q => q.Id == id);

            return productUnitType;
        }

        /// <summary>
        /// 取得所有產品單位
        /// </summary>
        /// <returns></returns>
        public List<ProductUnitType> GetAll()
        {
            IQueryable<ProductUnitType> query = _productUnitTypeRepository.GetAll();
            List<ProductUnitType> productUnitTypes = query.ToList();

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
                new ArgumentNullException(nameof(productUnitType));
            }

            try
            {
                await _productUnitTypeRepository.CreateAsync(productUnitType);
            }
            catch
            {
                throw;
            }
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

            try
            {
                await _productUnitTypeRepository.UpdateAsync(entity);
            }
            catch
            {
                throw;
            }
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

            try
            {
                await _productUnitTypeRepository.UpdateAsync(entity);
            }
            catch
            {
                throw;
            }
        }
    }
}