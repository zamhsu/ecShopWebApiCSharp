using Microsoft.EntityFrameworkCore;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices;
using WebApi.Models;
using WebApi.Models.Orders;

namespace WebApi.Base.Services
{
    public class CouponService : ICouponService
    {
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CouponService(IRepository<Coupon> couponRepository,
            IUnitOfWork unitOfWork,
            ILogger logger)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// 使用id取得一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <returns></returns>
        public async Task<Coupon> GetByIdAsync(int id)
        {
            int deleteStatus = (int)CouponStatusPara.Delete;
            Coupon coupon = await _couponRepository.GetAsync(q => q.Id == id && q.StatusId != deleteStatus);

            return coupon;
        }

        /// <summary>
        /// 使用優惠券代碼取得一筆優惠券
        /// </summary>
        /// <param name="code">優惠券代碼</param>
        /// <returns></returns>
        public async Task<Coupon> GetByCodeAsync(string code)
        {
            int deleteStatus = (int)CouponStatusPara.Delete;
            Coupon coupon = await _couponRepository.GetAsync(q => q.Code == code && q.StatusId != deleteStatus);

            return coupon;
        }

        /// <summary>
        /// 取得所有優惠券
        /// </summary>
        /// <returns></returns>
        public async Task<List<Coupon>> GetAllAsync()
        {
            int deleteStatus = (int)CouponStatusPara.Delete;
            IQueryable<Coupon> query = _couponRepository.GetAll().Where(q => q.StatusId != deleteStatus);
            List<Coupon> coupons = await query.ToListAsync();

            return coupons;
        }

        /// <summary>
        /// 新增一筆優惠券資料
        /// </summary>
        /// <param name="coupon">新增優惠券的資料</param>
        public async Task CreateAsync(Coupon coupon)
        {
            if (coupon == null)
            {
                _logger.LogInformation("[Create] Coupon can not be null");
                new ArgumentNullException(nameof(coupon));
            }

            coupon.StatusId = (int)CouponStatusPara.OK;

            try
            {
                await _couponRepository.CreateAsync(coupon);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 修改一筆優惠券資料
        /// </summary>
        /// <param name="id">優惠券id</param>
        /// <param name="coupon">修改優惠券的資料</param>
        public async Task UpdateAsync(int id, Coupon coupon)
        {
            Coupon enity = await GetByIdAsync(id);

            if (enity == null)
            {
                _logger.LogInformation($"[Update] Coupon is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(enity));
            }

            enity.Title = coupon.Title;
            enity.Used = coupon.Used;
            enity.CouponStatus = coupon.CouponStatus;

            try
            {
                _couponRepository.Update(enity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 使用id刪除一筆優惠券
        /// </summary>
        /// <param name="id">優惠券id</param>
        public async Task DeleteByGuidAsync(int id)
        {
            Coupon entity = await GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] Coupon is not existed (Id:{id})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.StatusId = (int)CouponStatusPara.Delete;

            try
            {
                _couponRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}