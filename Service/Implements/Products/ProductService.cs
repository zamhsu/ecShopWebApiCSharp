using AutoMapper;
using Common.Enums;
using Common.Extensions;
using Common.Helpers;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Products;
using Repository.Interfaces;
using Service.Dtos.Products;
using Service.Interfaces.Products;

namespace Service.Implements.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<Product> _logger;

        public ProductService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppLogger<Product> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用Guid取得一筆產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        public async Task<ProductDto> GetByGuidAsync(string guid)
        {
            int okStatus = (int)ProductStatusEnum.OK;
            Product product = await _unitOfWork.Repository<Product>()
                .GetAsync(q => q.Guid == guid && q.StatusId == okStatus);

            ProductDto dto = _mapper.Map<ProductDto>(product);

            return dto;
        }

        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        public async Task<ProductDetailDto> GetDetailByGuidAsync(string guid)
        {
            int okStatus = (int)ProductStatusEnum.OK;
            Product product = await _unitOfWork.Repository<Product>().GetAll()
                .Include(q => q.ProductCategoryType)
                .Include(q => q.ProductUnitType)
                .Include(q => q.ProductStatus)
                .FirstOrDefaultAsync(q => q.Guid == guid && q.StatusId == okStatus);

            ProductDetailDto dto = _mapper.Map<ProductDetailDto>(product);

            return dto;
        }

        /// <summary>
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetAllUsableAsync()
        {
            int okStatus = (int)ProductStatusEnum.OK;
            IQueryable<Product> query = _unitOfWork.Repository<Product>().GetAll()
                .Where(q => q.StatusId == okStatus);
            List<Product> products = await query.ToListAsync();

            List<ProductDto> dtos = _mapper.Map<List<ProductDto>>(products);

            return dtos;
        }

        /// <summary>
        /// 取得包含關聯性資料的所有產品
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductDetailDto>> GetDetailAllUsableAsync()
        {
            int okStatus = (int)ProductStatusEnum.OK;
            IQueryable<Product> query = _unitOfWork.Repository<Product>().GetAll()
                .Where(q => q.StatusId == okStatus)
                .Include(q => q.ProductCategoryType)
                .Include(q => q.ProductUnitType)
                .Include(q => q.ProductStatus);

            List<Product> products = await query.ToListAsync();

            List<ProductDetailDto> dtos = _mapper.Map<List<ProductDetailDto>>(products);

            return dtos;
        }

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有產品
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        public PagedList<ProductDetailDto> GetPagedDetailAllUsable(int pageSize, int page)
        {
            int okStatus = (int)ProductStatusEnum.OK;
            IQueryable<Product> query = _unitOfWork.Repository<Product>().GetAll()
                .Where(q => q.StatusId == okStatus)
                .Include(q => q.ProductCategoryType)
                .Include(q => q.ProductUnitType)
                .Include(q => q.ProductStatus);

            query = query.OrderByDescending(q => q.Id);

            PagedList<Product> products = query.ToPagedList(pageSize, page);

            PagedList<ProductDetailDto> dtos = _mapper.Map<PagedList<ProductDetailDto>>(products);

            return dtos;
        }

        /// <summary>
        /// 新增一筆產品資料
        /// </summary>
        /// <param name="createDto">新增產品的資料</param>
        public async Task<bool> CreateAsync(ProductCreateDto createDto)
        {
            if (createDto == null)
            {
                _logger.LogInformation("[Create] ProductCreateDto can not be null");
                throw new ArgumentNullException(nameof(createDto));
            }

            Product product = _mapper.Map<Product>(createDto);

            product.Guid = Guid.NewGuid().ToString();
            product.StartDisplay = product.StartDisplay.ToUniversalTime();
            product.EndDisplay = product.EndDisplay.ToUniversalTime();
            product.StatusId = (int)ProductStatusEnum.OK;
            product.CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            await _unitOfWork.Repository<Product>().CreateAsync(product);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆產品資料
        /// </summary>
        /// <param name="updateDto">修改產品的資料</param>
        public async Task<bool> UpdateAsync(ProductUpdateDto updateDto)
        {
            Product entity = await _unitOfWork.Repository<Product>()
                .GetAsync(q => q.Guid == updateDto.Guid && q.StatusId == (int)ProductStatusEnum.OK);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Product is not existed (Guid:{updateDto.Guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Title = updateDto.Title;
            entity.CategoryId = updateDto.CategoryId;
            entity.UnitId = updateDto.UnitId;
            entity.Quantity = updateDto.Quantity;
            entity.OriginPrice = updateDto.OriginPrice;
            entity.Price = updateDto.Price;
            entity.Description = updateDto.Description;
            entity.StartDisplay = updateDto.StartDisplay.ToUniversalTime();
            entity.EndDisplay = updateDto.EndDisplay.ToUniversalTime();
            entity.ImageUrl = updateDto.ImageUrl;
            entity.Memo = updateDto.Memo;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<Product>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 使用Guid刪除一筆產品
        /// </summary>
        /// <param name="guid">產品GUID</param>
        public async Task<bool> DeleteByGuidAsync(string guid)
        {
            Product entity = await _unitOfWork.Repository<Product>()
                .GetAsync(q => q.Guid == guid && q.StatusId == (int)ProductStatusEnum.OK);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] Product is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.StatusId = (int)ProductStatusEnum.Delete;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<Product>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 產品是否存在
        /// </summary>
        /// <param name="guid">產品GUID</param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(string guid)
        {
            bool result = await _unitOfWork.Repository<Product>().GetAllNoTracking()
                .AnyAsync(q => q.StatusId == (int)ProductStatusEnum.OK);

            return result;
        }
    }
}