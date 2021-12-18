using Microsoft.EntityFrameworkCore;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Orders;
using WebApi.Dtos.Orders;
using WebApi.Models;
using WebApi.Models.Orders;

namespace WebApi.Base.Services.Orders
{
    public class OrderSerivce : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderAmountService _orderAmountService;
        private readonly IAppLogger<Order> _logger;

        public OrderSerivce(IRepository<Order> orderRepository,
            IRepository<OrderDetail> orderDetailRepository,
            IRepository<Coupon> couponRepository,
            IUnitOfWork unitOfWork,
            IOrderAmountService orderAmountService,
            IAppLogger<Order> logger)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
            _orderAmountService = orderAmountService;
            _logger = logger;
        }

        /// <summary>
        /// 使用Guid取得一筆訂單
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<Order> GetByGuidAsync(string guid)
        {
            Order order = await _orderRepository.GetAsync(q => q.Guid == guid);

            return order;
        }

        /// <summary>
        /// 使用Guid取得一筆訂單詳細資料
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<Order?> GetDetailByGuidAsync(string guid)
        {
            Order? order = await _orderRepository.GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus)
                .FirstOrDefaultAsync(q => q.Guid == guid);

            return order;
        }

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetAllAsync()
        {
            List<Order> orders = await _orderRepository.GetAll().ToListAsync();

            return orders;
        }

        /// <summary>
        /// 取得所有訂單詳細資料
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetDetailAllAsync()
        {
            List<Order> orders = await _orderRepository.GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus)
                .ToListAsync();

            return orders;
        }

        /// <summary>
        /// 下訂單
        /// </summary>
        /// <param name="order">訂單資料</param>
        /// <param name="placeOrderDetails">商品</param>
        /// <param name="coupon">優惠券</param>
        /// <returns></returns>
        public async Task PlaceOrderAsync(Order order, List<PlaceOrderDetailModel> placeOrderDetails, Coupon? coupon)
        {
            int totalAmount = 0;
            int itemTotalAmount = 0;
            int discountAmount = 0;

            int itemIndex = 1;
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            // 建立OrderDetail
            foreach (var item in placeOrderDetails)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.Id;
                orderDetail.ItemNo = itemIndex;
                orderDetail.ProductId = item.ProductId;
                orderDetail.Quantity = item.Quantity;
                orderDetail.Total = _orderAmountService.CalculateItemTotal(item);

                orderDetails.Add(orderDetail);
                
                // 計算商品總額
                itemTotalAmount += orderDetail.Total;
                itemIndex++;
            }

            if (coupon != null)
            {
                // 計算優惠金額
                discountAmount = _orderAmountService.CalculateDiscountAmount(coupon, itemTotalAmount);
                coupon.Used += 1;

                // 建立Coupon的OrderDetail
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.Id;
                orderDetail.ItemNo = itemIndex;
                orderDetail.CouponId = coupon.Id;
                orderDetail.Quantity = 1;
                orderDetail.Total = discountAmount;

                orderDetails.Add(orderDetail);
            }

            // 計算應付金額
            totalAmount = _orderAmountService.CalculateTotalAmount(itemTotalAmount, discountAmount);

            // 建立訂單
            order.Guid = Guid.NewGuid().ToString();
            order.Total = totalAmount;
            order.StatusId = (int)OrderStatusPara.PlaceOrder;
            order.CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _orderRepository.CreateAsync(order);
                await _orderDetailRepository.CreateAsync(orderDetails);
                if (coupon != null)
                {
                    _couponRepository.Update(coupon);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 修改一筆訂單資料
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="order">修改訂單的資料</param>
        public async Task UpdateAsync(string guid, Order order)
        {
            Order entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            if (order.PaidDate != null)
            {
                entity.PaidDate = order.PaidDate.GetValueOrDefault().ToUniversalTime();
            }

            entity.Name = order.Name;
            entity.Email = order.Email;
            entity.Phone = order.Phone;
            entity.Address = order.Address;
            entity.Total = order.Total;
            entity.PaymentMethodId = order.PaymentMethodId;
            entity.StatusId = order.StatusId;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                _orderRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}