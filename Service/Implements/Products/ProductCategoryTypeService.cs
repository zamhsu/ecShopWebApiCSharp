using AutoMapper;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Products;
using Repository.Interfaces;
using Service.Dtos.Products;
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
        public async Task<ProductCategoryTypeDto> GetByIdAsync(int id)
        {
            ProductCategoryType productCategoryType = await _unitOfWork.Repository<ProductCategoryType>()
                .GetAsync(q => q.Id == id && q.Deleted == false);

            ProductCategoryTypeDto dto = _mapper.Map<ProductCategoryTypeDto>(productCategoryType);

            return dto;
        }

        /// <summary>
        /// 取得所有產品分類
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductCategoryTypeDto>> GetAllAsync()
        {
            IQueryable<ProductCategoryType> query = _unitOfWork.Repository<ProductCategoryType>().GetAll()
                .Where(q => q.Deleted == false);
            List<ProductCategoryType> productCategoryTypes = await query.ToListAsync();

            List<ProductCategoryTypeDto> dtos = _mapper.Map<List<ProductCategoryTypeDto>>(productCategoryTypes);

            return dtos;
        }

        /// <summary>
        /// 新增一筆產品分類資料
        /// </summary>
        /// <param name="createDto">新增產品分類的資料</param>
        public async Task<bool> CreateAsync(ProductCategoryTypeCreateDto createDto)
        {
            if (createDto == null)
            {
                _logger.LogInformation("[Create] ProductCategoryTypeCreateDto can not be null");
                throw new ArgumentNullException(nameof(createDto));
            }

            ProductCategoryType productCategoryType = _mapper.Map<ProductCategoryType>(createDto);
            productCategoryType.Deleted = false;

            await _unitOfWork.Repository<ProductCategoryType>().CreateAsync(productCategoryType);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆產品分類資料
        /// </summary>
        /// <param name="updateDto">修改產品分類的資料</param>
        public async Task<bool> UpdateAsync(ProductCategoryTypeUpdateDto updateDto)
        {
            ProductCategoryType entity = await _unitOfWork.Repository<ProductCategoryType>()
                .GetAsync(q => q.Id == updateDto.Id && q.Deleted == false);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] ProductCategoryType is not existed (Id:{updateDto.Id})");
                throw new ArgumentNullException(nameof(updateDto));
            }

            entity.Name = updateDto.Name;

            _unitOfWork.Repository<ProductCategoryType>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 使用產品分類編號刪除一筆產品分類
        /// </summary>
        /// <param name="id">產品分類編號</param>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            ProductCategoryType entity = await _unitOfWork.Repository<ProductCategoryType>()
                .GetAsync(q => q.Id == id && q.Deleted == false);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] ProductCategoryType is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Deleted = true;

            _unitOfWork.Repository<ProductCategoryType>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 指定產品分類是否存在
        /// </summary>
        /// <param name="id">產品分類編號</param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(int id)
        {
            bool result = await _unitOfWork.Repository<ProductCategoryType>().GetAllNoTracking()
                .AnyAsync(q => q.Id == id && q.Deleted == false);

            return result;
        }
    }
}