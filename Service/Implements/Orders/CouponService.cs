using Microsoft.EntityFrameworkCore;
using Common.Extensions;
using Service.Interfaces.Orders;
using Repository.Interfaces;
using Repository.Entities.Orders;
using Common.Interfaces;
using Common.Enums;
using Common.Helpers;
using Service.Dtos.Orders;
using AutoMapper;

namespace Service.Implments.Orders
{
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<Coupon> _logger;

        public CouponService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppLogger<Coupon> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用id取得一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        public async Task<CouponDto> GetByIdAsync(int id)
        {
            int deleteStatus = (int)CouponStatusEnum.Delete;
            Coupon coupon = await _unitOfWork.Repository<Coupon>()
                .GetAsync(q => q.Id.Equals(id) && q.StatusId != deleteStatus);

            CouponDto dto = _mapper.Map<CouponDto>(coupon);

            return dto;
        }

        /// <summary>
        /// 使用id取得一筆包含關聯性資料的優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        public async Task<CouponDetailDto> GetDetailByIdAsync(int id)
        {
            int deleteStatus = (int)CouponStatusEnum.Delete;
            Coupon coupon = await _unitOfWork.Repository<Coupon>().GetAll()
                .Include(q => q.CouponStatus)
                .FirstOrDefaultAsync(q => q.Id.Equals(id) && q.StatusId != deleteStatus);

            CouponDetailDto dto = _mapper.Map<CouponDetailDto>(coupon);

            return dto;
        }

        /// <summary>
        /// 使用優惠券代碼取得一筆優惠券
        /// </summary>
        /// <param name="code">優惠券代碼</param>
        /// <returns></returns>
        public async Task<CouponDto> GetByCodeAsync(string code)
        {
            int deleteStatus = (int)CouponStatusEnum.Delete;
            Coupon coupon = await _unitOfWork.Repository<Coupon>()
                .GetAsync(q => q.Code.Equals(code) && q.StatusId != deleteStatus);

            CouponDto dto = _mapper.Map<CouponDto>(coupon);

            return dto;
        }

        /// <summary>
        /// 使用優惠券代碼取得一筆可用的優惠券
        /// </summary>
        /// <param name="code">優惠券代碼</param>
        /// <returns></returns>
        public async Task<CouponDto> GetUsableByCodeAsync(string code)
        {
            int deleteStatus = (int)CouponStatusEnum.Delete;
            Coupon coupon = await _unitOfWork.Repository<Coupon>()
                .GetAsync(q => q.Code.Equals(code) 
                            && q.Quantity > q.Used 
                            && q.StatusId != deleteStatus);

            CouponDto dto = _mapper.Map<CouponDto>(coupon);

            return dto;
        }

        /// <summary>
        /// 取得所有優惠券
        /// </summary>
        /// <returns></returns>
        public async Task<List<CouponDto>> GetAllAsync()
        {
            int deleteStatus = (int)CouponStatusEnum.Delete;
            IQueryable<Coupon> query = _unitOfWork.Repository<Coupon>().GetAll()
                .Where(q => q.StatusId != deleteStatus);
            List<Coupon> coupons = await query.ToListAsync();

            List<CouponDto> dtos = _mapper.Map<List<CouponDto>>(coupons);

            return dtos;
        }

        /// <summary>
        /// 取得包含關聯性資料的所有優惠券
        /// </summary>
        /// <returns></returns>
        public async Task<List<CouponDetailDto>> GetDetailAllAsync()
        {
            int okStatus = (int)CouponStatusEnum.OK;
            IQueryable<Coupon> query = _unitOfWork.Repository<Coupon>().GetAll()
                .Where(q => q.StatusId.Equals(okStatus))
                .Include(q => q.CouponStatus);

            List<Coupon> coupons = await query.ToListAsync();

            List<CouponDetailDto> dtos = _mapper.Map<List<CouponDetailDto>>(coupons);

            return dtos;
        }

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有優惠券
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        public PagedList<CouponDetailDto> GetPagedDetailAll(int pageSize, int page)
        {
            int okStatus = (int)CouponStatusEnum.OK;
            IQueryable<Coupon> query = _unitOfWork.Repository<Coupon>().GetAll()
                .Where(q => q.StatusId.Equals(okStatus))
                .Include(q => q.CouponStatus);

            query = query.OrderByDescending(q => q.Id);

            PagedList<Coupon> coupons = query.ToPagedList(pageSize, page);

            PagedList<CouponDetailDto> dtos = _mapper.Map<PagedList<CouponDetailDto>>(coupons);

            return dtos;
        }

        /// <summary>
        /// 新增一筆優惠券資料
        /// </summary>
        /// <param name="createDto">新增優惠券的資料</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(CouponCreateDto createDto)
        {
            if (createDto is null)
            {
                _logger.LogInformation("[Create] CouponCreateDto can not be null");
                throw new ArgumentNullException(nameof(createDto));
            }

            Coupon coupon = _mapper.Map<Coupon>(createDto);

            coupon.StartDate = coupon.StartDate.ToUniversalTime();
            coupon.ExpiredDate = coupon.ExpiredDate.ToUniversalTime();
            coupon.StatusId = (int)CouponStatusEnum.OK;

            await _unitOfWork.Repository<Coupon>().CreateAsync(coupon);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆優惠券資料
        /// </summary>
        /// <param name="updateDto">修改優惠券的資料</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(CouponUpdateDto updateDto)
        {
            int deleteStatus = (int)CouponStatusEnum.Delete;
            Coupon entity = await _unitOfWork.Repository<Coupon>()
                .GetAsync(q => q.Id.Equals(updateDto.Id)
                            && q.StatusId != deleteStatus);

            if (entity is null)
            {
                _logger.LogInformation($"[Update] Coupon is not existed (Id:{updateDto.Id})");
                throw new ArgumentNullException(nameof(updateDto));
            }

            entity.Title = updateDto.Title;
            entity.Used = updateDto.Used;
            entity.StatusId = updateDto.StatusId;

            _unitOfWork.Repository<Coupon>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 使用id刪除一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            int deleteStatus = (int)CouponStatusEnum.Delete;
            Coupon entity = await _unitOfWork.Repository<Coupon>()
                .GetAsync(q => q.Id.Equals(id) 
                            && q.StatusId != deleteStatus);

            if (entity is null)
            {
                _logger.LogInformation($"[Delete] Coupon is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.StatusId = (int)CouponStatusEnum.Delete;

            _unitOfWork.Repository<Coupon>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}