using AutoMapper;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Products;
using Repository.Interfaces;
using Service.Dtos.Products;
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
        public async Task<ProductUnitTypeDto> GetByIdAsync(int id)
        {
            ProductUnitType productUnitType = await _unitOfWork.Repository<ProductUnitType>()
                .GetAsync(q => q.Id == id && q.Deleted == false);

            ProductUnitTypeDto dto = _mapper.Map<ProductUnitTypeDto>(productUnitType);

            return dto;
        }

        /// <summary>
        /// 取得所有產品單位
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductUnitTypeDto>> GetAllAsync()
        {
            IQueryable<ProductUnitType> query = _unitOfWork.Repository<ProductUnitType>().GetAll()
                .Where(q => q.Deleted == false);
            List<ProductUnitType> productUnitTypes = await query.ToListAsync();

            List<ProductUnitTypeDto> dtos = _mapper.Map<List<ProductUnitTypeDto>>(productUnitTypes);

            return dtos;
        }

        /// <summary>
        /// 新增一筆產品單位資料
        /// </summary>
        /// <param name="createDto">新增產品單位的資料</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(ProductUnitTypeCreateDto createDto)
        {
            if (createDto == null)
            {
                _logger.LogInformation("[Create] ProductUnitTypeCreateDto can not be null");
                throw new ArgumentNullException(nameof(createDto));
            }

            ProductUnitType productUnitType = _mapper.Map<ProductUnitType>(createDto);
            productUnitType.Deleted = false;

            await _unitOfWork.Repository<ProductUnitType>().CreateAsync(productUnitType);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆產品單位資料
        /// </summary>
        /// <param name="UpdateDto">修改產品單位的資料</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ProductUnitTypeUpdateDto updateDto)
        {
            ProductUnitType entity = await _unitOfWork.Repository<ProductUnitType>()
                .GetAsync(q => q.Id == updateDto.Id && q.Deleted == false);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] ProductUnitType is not existed (Id:{updateDto.Id})");
                throw new ArgumentNullException(nameof(updateDto));
            }

            entity.Name = updateDto.Name;

            _unitOfWork.Repository<ProductUnitType>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 使用產品單位編號刪除一筆產品單位
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            ProductUnitType entity = await _unitOfWork.Repository<ProductUnitType>()
                .GetAsync(q => q.Id == id && q.Deleted == false);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] ProductUnitType is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Deleted = true;

            _unitOfWork.Repository<ProductUnitType>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 產品單位是否存在
        /// </summary>
        /// <param name="id">產品單位編號</param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(int id)
        {
            bool result = await _unitOfWork.Repository<ProductUnitType>().GetAllNoTracking()
                .AnyAsync(q => q.Id == id && q.Deleted == false);

            return result;
        }
    }
}