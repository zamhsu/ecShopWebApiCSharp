using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Products;
using WebApi.Models;
using WebApi.Models.Products;

namespace WebApi.Base.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<Product> _logger;

        public ProductService(IRepository<Product> productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppLogger<Product> logger)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
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
            Product product = await _productRepository.GetAll()
                .Include(q => q.ProductCategoryType)
                .Include(q => q.ProductUnitType)
                .Include(q => q.ProductStatus)
                .FirstOrDefaultAsync(q => q.Guid == guid && q.StatusId == okStatus);

            return product;
        }

        /// <summary>
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetAllUsableAsync()
        {
            int okStatus = (int)ProductStatusPara.OK;
            IQueryable<Product> query = _productRepository.GetAll().Where(q => q.StatusId == okStatus);
            List<Product> products = await query.ToListAsync();

            return products;
        }

        /// <summary>
        /// 取得包含關聯性資料的所有產品
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetDetailAllUsableAsync()
        {
            int okStatus = (int)ProductStatusPara.OK;
            IQueryable<Product> query = _productRepository.GetAll()
                .Where(q => q.StatusId == okStatus)
                .Include(q => q.ProductCategoryType)
                .Include(q => q.ProductUnitType)
                .Include(q => q.ProductStatus);
            
            List<Product> products = await query.ToListAsync();

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
                await _unitOfWork.SaveChangesAsync();
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
            Product entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Product is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Title = product.Title;
            entity.CategoryId = product.CategoryId;
            entity.UnitId = product.UnitId;
            entity.Quantity = product.Quantity;
            entity.OriginPrice = product.OriginPrice;
            entity.Price = product.Price;
            entity.Description = product.Description;
            entity.StartDisplay = product.StartDisplay.ToUniversalTime();
            entity.EndDisplay = product.EndDisplay.ToUniversalTime();
            entity.ImageUrl = product.ImageUrl;
            entity.Memo = product.Memo;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                _productRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
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

            entity.StatusId = (int)ProductStatusPara.Delete;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                _productRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}