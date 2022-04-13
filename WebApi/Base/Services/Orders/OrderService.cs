using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Orders;
using WebApi.Base.IServices.Products;
using WebApi.Dtos;
using WebApi.Dtos.Orders;
using WebApi.Extensions;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Products;

namespace WebApi.Base.Services.Orders
{
    public class OrderSerivce : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderAmountService _orderAmountService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IAppLogger<Order> _logger;

        public OrderSerivce(IRepository<Order> orderRepository,
            IRepository<OrderDetail> orderDetailRepository,
            IRepository<Coupon> couponRepository,
            IUnitOfWork unitOfWork,
            IOrderAmountService orderAmountService,
            IOrderDetailService orderDetailService,
            IProductService productService,
            IMapper mapper,
            IAppLogger<Order> logger)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
            _orderAmountService = orderAmountService;
            _orderDetailService = orderDetailService;
            _productService = productService;
            _mapper = mapper;
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
        public async Task<OrderDisplayDetailModel?> GetDetailByGuidAsync(string guid)
        {
            Order? order = await _orderRepository.GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus)
                .FirstOrDefaultAsync(q => q.Guid == guid);

            if (order == null)
            {
                return null;
            }

            List<OrderDetail> itemDetail = await _orderDetailService.GetAllItemDetailByOrderIdAsync(order.Id);
            OrderDetail? couponDetail = await _orderDetailService.GetCouponDetailByOrderIdAsync(order.Id);

            OrderDisplayDetailModel displayModel = _mapper.Map<OrderDisplayDetailModel>(order);
            displayModel.OrderDetails = _mapper.Map<List<OrderItemDetailDisplayModel>>(itemDetail);

            if (couponDetail != null)
            {
                displayModel.CouponDetail = _mapper.Map<OrderCouponDetailDisplayModel>(couponDetail);
            }

            return displayModel;
        }

        /// <summary>
        /// 使用消費者資料取得分頁後的訂單詳細資料
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <param name="name">姓名</param>
        /// <param name="email">Email</param>
        /// <param name="phone">聯絡電話</param>
        /// <returns></returns>
        public async Task<PagedList<OrderDisplayDetailModel>> GetPagedDetailByCustomerInfoAsync(int pageSize, int page, string name, string email, string phone)
        {
            IQueryable<Order> query = _orderRepository.GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus)
                .Where(q => q.Name == name
                         && q.Email == email
                         && q.Phone == phone)
                .OrderByDescending(q => q.Id);

            PagedList<Order> pagedOrders = query.ToPagedList(pageSize, page);

            List<OrderDisplayDetailModel> displayModels = new List<OrderDisplayDetailModel>();

            foreach (var order in pagedOrders.PagedData)
            {
                List<OrderDetail> itemDetail = await _orderDetailService.GetAllItemDetailByOrderIdAsync(order.Id);
                OrderDetail? couponDetail = await _orderDetailService.GetCouponDetailByOrderIdAsync(order.Id);

                OrderDisplayDetailModel displayModel = _mapper.Map<OrderDisplayDetailModel>(order);
                displayModel.OrderDetails = _mapper.Map<List<OrderItemDetailDisplayModel>>(itemDetail);

                if (couponDetail != null)
                {
                    displayModel.CouponDetail = _mapper.Map<OrderCouponDetailDisplayModel>(couponDetail);
                }

                displayModels.Add(displayModel);
            }

            PagedList<OrderDisplayDetailModel> pagedDisplayModels = new PagedList<OrderDisplayDetailModel>(displayModels, pageSize ,page);

            return pagedDisplayModels;
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
        /// 取得分頁後所有訂單詳細資料
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        public PagedList<Order> GetPagedDetailAll(int pageSize, int page)
        {
            IQueryable<Order> query = _orderRepository.GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus);

            query = query.OrderByDescending(q => q.Id);

            PagedList<Order> orders = query.ToPagedList(pageSize, page);

            return orders;
        }

        /// <summary>
        /// 下訂單
        /// </summary>
        /// <param name="order">訂單資料</param>
        /// <param name="placeOrderDetails">商品</param>
        /// <param name="coupon">優惠券</param>
        /// <returns></returns>
        public async Task<string> PlaceOrderAsync(Order order, List<PlaceOrderDetailModel> placeOrderDetails, Coupon? coupon)
        {
            int totalAmount = 0;
            int itemTotalAmount = 0;
            int discountAmount = 0;

            int itemIndex = 1;
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            // 建立OrderDetail
            foreach (var item in placeOrderDetails)
            {
                Product product = await _productService.GetByGuidAsync(item.ProductGuid);

                if (product == null) continue;

                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.Id;
                orderDetail.ItemNo = itemIndex;
                orderDetail.ProductId = product.Id;
                orderDetail.Quantity = item.Quantity;
                orderDetail.Total = _orderAmountService.CalculateItemTotal(item);
                orderDetail.Order = order;
                orderDetail.Product = await _productService.GetByGuidAsync(item.ProductGuid);

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
                orderDetail.Order = order;
                orderDetail.Coupon = coupon;

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

                return order.Guid;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 修改一筆訂單裡的消費者個人資料
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="order">修改訂單的資料</param>
        public async Task UpdateCustomerInfoAsync(string guid, Order order)
        {
            Order entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Name = order.Name;
            entity.Email = order.Email;
            entity.Phone = order.Phone;
            entity.Address = order.Address;
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

        /// <summary>
        /// 修改一筆訂單狀態為完成付款
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="paymentMethodPara">付款方式</param>
        public async Task UpdateStatusToPaymentSuccessfulAsync(string guid, PaymentMethodPara paymentMethodPara)
        {
            Order entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.PaymentMethodId = (int)paymentMethodPara;
            entity.PaidDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();
            entity.StatusId = (int)OrderStatusPara.PaymentSuccessful;
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

        /// <summary>
        /// 修改一筆訂單狀態為付款失敗
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="paymentMethodPara">付款方式</param>
        public async Task UpdateStatusToPaymentFailedAsync(string guid, PaymentMethodPara paymentMethodPara)
        {
            Order entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.PaymentMethodId = (int)paymentMethodPara;
            entity.PaidDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();
            entity.StatusId = (int)OrderStatusPara.PaymentFailed;
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
            entity.PaidDate = order.PaidDate;
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

        /// <summary>
        /// 訂單是否已經付款
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<bool> IsOrderPaidAsync(string guid)
        {
            Order order = await GetByGuidAsync(guid);

            return order.StatusId > (int)OrderStatusPara.PlaceOrder;
        }
    }
}