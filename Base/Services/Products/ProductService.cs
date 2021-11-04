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
        private readonly IRepository<Product> _productRepository;
        private readonly IAppLogger<Product> _logger;

        public ProductService(IRepository<Product> productRepository,
            IAppLogger<Product> logger)
        {
            _productRepository = productRepository;
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
        /// 新增一筆產品資料
        /// </summary>
        /// <param name="createProductModel">新增產品的資料</param>
        /// <param name="userTimeZone">使用者時區</param>
        public async Task CreateAsync(CreateProductModel createProductModel, TimeSpan userTimeZone)
        {
            if (createProductModel == null)
            {
                _logger.LogInformation("[Create] CreateProductModel can not be null");
                new ArgumentNullException(nameof(createProductModel));
            }

            var startOffset = new DateTimeOffset(createProductModel.StartDisplay, userTimeZone);
            var endOffset = new DateTimeOffset(createProductModel.EndDisplay, userTimeZone);

            Product product = new Product()
            {
                Guid = Guid.NewGuid().ToString(),
                Title = createProductModel.Title,
                CategoryId = createProductModel.CategoryId,
                UnitId = createProductModel.UnitId,
                Quantity = createProductModel.Quantity,
                OriginPrice = createProductModel.OriginPrice,
                Price = createProductModel.Price,
                Description = createProductModel.Description,
                StartDisplay = startOffset.ToUniversalTime(),
                EndDisplay = endOffset.ToUniversalTime(),
                ImageUrl = createProductModel.ImageUrl,
                Memo = createProductModel.Memo,
                StatusId = (int)ProductStatusPara.OK,
                CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime()
            };

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
        /// <param name="updateProductModel">修改產品的資料</param>
        /// <param name="userTimeZone">使用者時區</param>
        public async Task UpdateAsync(string guid, UpdateProductModel updateProductModel, TimeSpan userTimeZone)
        {
            Product product = await GetByGuidAsync(guid);

            if (product == null)
            {
                _logger.LogInformation($"[Update] Product is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(product));
            }

            var startOffset = new DateTimeOffset(updateProductModel.StartDisplay, userTimeZone);
            var endOffset = new DateTimeOffset(updateProductModel.EndDisplay, userTimeZone);

            product.Title = updateProductModel.Title;
            product.CategoryId = updateProductModel.CategoryId;
            product.UnitId = updateProductModel.UnitId;
            product.Quantity = updateProductModel.Quantity;
            product.OriginPrice = updateProductModel.OriginPrice;
            product.Price = updateProductModel.Price;
            product.Description = updateProductModel.Description;
            product.StartDisplay = startOffset.ToUniversalTime();
            product.EndDisplay = endOffset.ToUniversalTime();
            product.ImageUrl = updateProductModel.ImageUrl;
            product.Memo = updateProductModel.Memo;

            try
            {
                await _productRepository.UpdateAsync(product);
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
            Product product = await GetByGuidAsync(guid);

            if (product == null)
            {
                _logger.LogInformation($"[Delete] Product is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(product));
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