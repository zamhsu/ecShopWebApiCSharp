using AutoMapper;
using Common.Enums;
using Common.Extensions;
using Common.Helpers;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Orders;
using Repository.Entities.Products;
using Repository.Interfaces;
using Service.Dtos.Orders;
using Service.Interfaces.Orders;
using Service.Interfaces.Products;

namespace Service.Implments.Orders
{
    public class OrderSerivce : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderAmountService _orderAmountService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IAppLogger<Order> _logger;

        public OrderSerivce(IUnitOfWork unitOfWork,
            IOrderAmountService orderAmountService,
            IOrderDetailService orderDetailService,
            IProductService productService,
            IMapper mapper,
            IAppLogger<Order> logger)
        {
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
            Order order = await _unitOfWork.Repository<Order>()
                .GetAsync(q => q.Guid == guid);

            return order;
        }

        /// <summary>
        /// 使用Guid取得一筆訂單詳細資料
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<OrderDisplayDetailDto> GetDetailByGuidAsync(string guid)
        {
            Order order = await _unitOfWork.Repository<Order>().GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus)
                .FirstOrDefaultAsync(q => q.Guid == guid);

            if (order == null)
            {
                return null;
            }

            List<OrderDetail> itemDetail = await _orderDetailService.GetAllItemDetailByOrderIdAsync(order.Id);
            OrderDetail couponDetail = await _orderDetailService.GetCouponDetailByOrderIdAsync(order.Id);

            OrderDisplayDetailDto displayDto = _mapper.Map<OrderDisplayDetailDto>(order);
            displayDto.OrderDetails = _mapper.Map<List<OrderItemDetailDisplayDto>>(itemDetail);

            if (couponDetail != null)
            {
                displayDto.CouponDetail = _mapper.Map<OrderCouponDetailDisplayDto>(couponDetail);
            }

            return displayDto;
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
        public async Task<PagedList<OrderDisplayDetailDto>> GetPagedDetailByCustomerInfoAsync(int pageSize, int page, string name, string email, string phone)
        {
            IQueryable<Order> query = _unitOfWork.Repository<Order>().GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus)
                .Where(q => q.Name == name
                         && q.Email == email
                         && q.Phone == phone)
                .OrderByDescending(q => q.Id);

            PagedList<Order> pagedOrders = query.ToPagedList(pageSize, page);

            List<OrderDisplayDetailDto> displayDtos = new List<OrderDisplayDetailDto>();

            foreach (var order in pagedOrders.PagedData)
            {
                List<OrderDetail> itemDetail = await _orderDetailService.GetAllItemDetailByOrderIdAsync(order.Id);
                OrderDetail couponDetail = await _orderDetailService.GetCouponDetailByOrderIdAsync(order.Id);

                OrderDisplayDetailDto displayDto = _mapper.Map<OrderDisplayDetailDto>(order);
                displayDto.OrderDetails = _mapper.Map<List<OrderItemDetailDisplayDto>>(itemDetail);

                if (couponDetail != null)
                {
                    displayDto.CouponDetail = _mapper.Map<OrderCouponDetailDisplayDto>(couponDetail);
                }

                displayDtos.Add(displayDto);
            }

            PagedList<OrderDisplayDetailDto> pagedDisplayDtos = new PagedList<OrderDisplayDetailDto>()
            {
                PagedData = displayDtos,
                Pagination = pagedOrders.Pagination
            };

            return pagedDisplayDtos;
        }

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetAllAsync()
        {
            List<Order> orders = await _unitOfWork.Repository<Order>().GetAll().ToListAsync();

            return orders;
        }

        /// <summary>
        /// 取得所有訂單詳細資料
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetDetailAllAsync()
        {
            List<Order> orders = await _unitOfWork.Repository<Order>().GetAll()
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
            IQueryable<Order> query = _unitOfWork.Repository<Order>().GetAll()
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
        public async Task<string> PlaceOrderAsync(Order order, List<PlaceOrderDetailDto> placeOrderDetails, Coupon coupon)
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
            order.StatusId = (int)OrderStatusEnum.PlaceOrder;
            order.CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            await _unitOfWork.Repository<Order>().CreateAsync(order);
            await _unitOfWork.Repository<OrderDetail>().CreateAsync(orderDetails);
            if (coupon != null)
            {
                _unitOfWork.Repository<Coupon>().Update(coupon);
            }

            await _unitOfWork.SaveChangesAsync();

            return order.Guid;
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

            _unitOfWork.Repository<Order>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改一筆訂單狀態為完成付款
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="paymentMethod">付款方式</param>
        public async Task UpdateStatusToPaymentSuccessfulAsync(string guid, PaymentMethodEnum paymentMethod)
        {
            Order entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.PaymentMethodId = (int)paymentMethod;
            entity.PaidDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();
            entity.StatusId = (int)OrderStatusEnum.PaymentSuccessful;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<Order>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改一筆訂單狀態為付款失敗
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="paymentMethod">付款方式</param>
        public async Task UpdateStatusToPaymentFailedAsync(string guid, PaymentMethodEnum paymentMethod)
        {
            Order entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.PaymentMethodId = (int)paymentMethod;
            entity.PaidDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();
            entity.StatusId = (int)OrderStatusEnum.PaymentFailed;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<Order>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
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

            _unitOfWork.Repository<Order>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 訂單是否已經付款
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<bool> IsOrderPaidAsync(string guid)
        {
            Order order = await GetByGuidAsync(guid);

            return order.StatusId > (int)OrderStatusEnum.PlaceOrder;
        }
    }
}