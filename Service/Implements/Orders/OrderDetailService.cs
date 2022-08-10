using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Orders;
using Repository.Interfaces;
using Service.Interfaces.Orders;

namespace Service.Implments.Orders
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<OrderDetail> _logger;

        public OrderDetailService(IUnitOfWork unitOfWork,
            IAppLogger<OrderDetail> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// 取得訂單商品詳細資料
        /// </summary>
        /// <param name="orderId">訂單編號</param>
        /// <returns></returns>
        public async Task<List<OrderDetail>> GetAllItemDetailByOrderIdAsync(int orderId)
        {
            IQueryable<OrderDetail> query = _unitOfWork.Repository<OrderDetail>().GetAll()
                .Include(q => q.Product)
                .Where(q => q.OrderId == orderId 
                         && q.ProductId != null 
                         && q.CouponId == null);

            List<OrderDetail> orderDetails = await query.ToListAsync();

            return orderDetails;
        }

        /// <summary>
        /// 取得訂單優惠券詳細資料
        /// </summary>
        /// <param name="orderId">訂單編號</param>
        /// <returns></returns>
        public async Task<OrderDetail> GetCouponDetailByOrderIdAsync(int orderId)
        {
            OrderDetail orderDetail = await _unitOfWork.Repository<OrderDetail>().GetAll()
                .Include(q => q.Coupon)
                .FirstOrDefaultAsync(q => q.OrderId == orderId 
                                       && q.ProductId == null 
                                       && q.CouponId != null);

            return orderDetail;
        }
    }
}