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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<Product> _logger;

        public ProductService(IProductRepository productRepository,
            IMapper mapper,
            IAppLogger<Product> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用Guid取得一筆產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        public async Task<Product> GetByGuidAsync(string guid)
        {
            int okStatus = (int)ProductStatusPara.OK;
            Product product = await _productRepository.GetAsync(q => q.Guid == guid && q.StatusId == okStatus);

            return product;
        }

        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        public async Task<Product> GetDetailByGuidAsync(string guid)
        {
            int okStatus = (int)ProductStatusPara.OK;
            Product product = await _productRepository.GetDetailAsync(q => q.Guid == guid && q.StatusId == okStatus);

            return product;
        }

        /// <summary>
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllUsable()
        {
            int okStatus = (int)ProductStatusPara.OK;
            IQueryable<Product> query = _productRepository.GetAll().Where(q => q.StatusId == okStatus);
            List<Product> products = query.ToList();

            return products;
        }

        /// <summary>
        /// 取得包含關聯性資料的所有產品
        /// </summary>
        /// <returns></returns>
        public List<Product> GetDetailAllUsable()
        {
            int okStatus = (int)ProductStatusPara.OK;
            IQueryable<Product> query = _productRepository.GetDetailAll().Where(q => q.StatusId == okStatus);
            List<Product> products = query.ToList();

            return products;
        }

        /// <summary>
        /// 新增一筆產品資料
        /// </summary>
        /// <param name="product">新增產品的資料</param>
        public async Task CreateAsync(Product product)
        {
            if (product == null)
            {
                _logger.LogInformation("[Create] Product can not be null");
                new ArgumentNullException(nameof(product));
            }

            product.Guid = Guid.NewGuid().ToString();
            product.StartDisplay = product.StartDisplay.ToUniversalTime();
            product.EndDisplay = product.EndDisplay.ToUniversalTime();
            product.StatusId = (int)ProductStatusPara.OK;
            product.CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _productRepository.CreateAsync(product);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 修改一筆產品資料
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <param name="product">修改產品的資料</param>
        public async Task UpdateAsync(string guid, Product product)
        {
            Product enity = await GetByGuidAsync(guid);

            if (enity == null)
            {
                _logger.LogInformation($"[Update] Product is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(enity));
            }

            enity.Title = product.Title;
            enity.CategoryId = product.CategoryId;
            enity.UnitId = product.UnitId;
            enity.Quantity = product.Quantity;
            enity.OriginPrice = product.OriginPrice;
            enity.Price = product.Price;
            enity.Description = product.Description;
            enity.StartDisplay = product.StartDisplay.ToUniversalTime();
            enity.EndDisplay = product.EndDisplay.ToUniversalTime();
            enity.ImageUrl = product.ImageUrl;
            enity.Memo = product.Memo;
            enity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _productRepository.UpdateAsync(enity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 使用Guid刪除一筆產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        public async Task DeleteByGuidAsync(string guid)
        {
            Product entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] Product is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            product.StatusId = (int)ProductStatusPara.Delete;
            product.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _productRepository.UpdateAsync(product);
            }
            catch
            {
                throw;
            }
        }
    }
}