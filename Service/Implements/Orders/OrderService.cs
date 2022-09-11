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

namespace Service.Implements.Orders
{
    public class OrderSerivce : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderAmountService _orderAmountService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;
        private readonly IAppLogger<Order> _logger;

        public OrderSerivce(IUnitOfWork unitOfWork,
            IOrderAmountService orderAmountService,
            IOrderDetailService orderDetailService,
            IMapper mapper,
            IAppLogger<Order> logger)
        {
            _unitOfWork = unitOfWork;
            _orderAmountService = orderAmountService;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用Guid取得一筆訂單
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<OrderDto> GetByGuidAsync(string guid)
        {
            Order order = await _unitOfWork.Repository<Order>()
                .GetAsync(q => q.Guid == guid);

            OrderDto dto = _mapper.Map<OrderDto>(order);

            return dto;
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

            List<OrderItemDetailDisplayDto> itemDetail = await _orderDetailService.GetAllItemDetailByOrderIdAsync(order.Id);
            OrderCouponDetailDisplayDto couponDetail = await _orderDetailService.GetCouponDetailByOrderIdAsync(order.Id);

            OrderDisplayDetailDto displayDto = _mapper.Map<OrderDisplayDetailDto>(order);
            displayDto.OrderDetails = itemDetail;

            if (couponDetail != null)
            {
                displayDto.CouponDetail = couponDetail;
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
                List<OrderItemDetailDisplayDto> itemDetail = await _orderDetailService.GetAllItemDetailByOrderIdAsync(order.Id);
                OrderCouponDetailDisplayDto couponDetail = await _orderDetailService.GetCouponDetailByOrderIdAsync(order.Id);

                OrderDisplayDetailDto displayDto = _mapper.Map<OrderDisplayDetailDto>(order);
                displayDto.OrderDetails = itemDetail;

                if (couponDetail != null)
                {
                    displayDto.CouponDetail = couponDetail;
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
        public async Task<List<OrderDto>> GetAllAsync()
        {
            List<Order> orders = await _unitOfWork.Repository<Order>().GetAll().ToListAsync();

            List<OrderDto> dtos = _mapper.Map<List<OrderDto>>(orders);

            return dtos;
        }

        /// <summary>
        /// 取得所有訂單詳細資料
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderDetailDto>> GetDetailAllAsync()
        {
            List<Order> orders = await _unitOfWork.Repository<Order>().GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus)
                .ToListAsync();

            List<OrderDetailDto> dtos = _mapper.Map<List<OrderDetailDto>>(orders);

            return dtos;
        }

        /// <summary>
        /// 取得分頁後所有訂單詳細資料
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        public PagedList<OrderDetailDto> GetPagedDetailAll(int pageSize, int page)
        {
            IQueryable<Order> query = _unitOfWork.Repository<Order>().GetAll()
                .Include(q => q.PaymentMethod)
                .Include(q => q.OrderStatus);

            query = query.OrderByDescending(q => q.Id);

            PagedList<Order> orders = query.ToPagedList(pageSize, page);
            PagedList<OrderDetailDto> dtos = new PagedList<OrderDetailDto>()
            {
                PagedData = _mapper.Map<List<OrderDetailDto>>(orders.PagedData),
                Pagination = orders.Pagination
            };

            return dtos;
        }

        /// <summary>
        /// 下訂單
        /// </summary>
        /// <param name="customerInfoDto">顧客資料</param>
        /// <param name="placeOrderDetails">商品</param>
        /// <param name="couponCode">優惠券代碼</param>
        /// <returns></returns>
        public async Task<string> PlaceOrderAsync(OrderCustomerInfoDto customerInfoDto, List<PlaceOrderDetailDto> placeOrderDetails, string couponCode)
        {
            int totalAmount = 0;
            int itemTotalAmount = 0;
            int discountAmount = 0;

            int itemIndex = 1;
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            Order order = _mapper.Map<Order>(customerInfoDto);

            // 建立OrderDetail
            foreach (var item in placeOrderDetails)
            {
                Product product = await _unitOfWork.Repository<Product>()
                    .GetAsync(q => q.Guid == item.ProductGuid && q.StatusId == (int)ProductStatusEnum.OK);

                if (product == null) continue;

                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.Id;
                orderDetail.ItemNo = itemIndex;
                orderDetail.ProductId = product.Id;
                orderDetail.Quantity = item.Quantity;
                orderDetail.Total = _orderAmountService.CalculateItemTotal(item);
                orderDetail.Order = order;
                orderDetail.Product = product;

                orderDetails.Add(orderDetail);

                // 計算商品總額
                itemTotalAmount += orderDetail.Total;
                itemIndex++;
            }

            Coupon coupon = await _unitOfWork.Repository<Coupon>()
                .GetAsync(q => q.Code == couponCode
                            && q.Quantity > q.Used 
                            && q.StatusId != (int)CouponStatusEnum.Delete);

            if (coupon is not null)
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
        /// <param name="customerInfoDto">修改訂單的資料</param>
        /// <returns></returns>
        public async Task<bool> UpdateCustomerInfoAsync(OrderCustomerInfoDto customerInfoDto)
        {
            Order entity = await _unitOfWork.Repository<Order>()
                .GetAsync(q => q.Guid == customerInfoDto.Guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{customerInfoDto.Guid})");
                throw new ArgumentNullException(nameof(customerInfoDto));
            }

            entity.Name = customerInfoDto.Name;
            entity.Email = customerInfoDto.Email;
            entity.Phone = customerInfoDto.Phone;
            entity.Address = customerInfoDto.Address;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<Order>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆訂單付款狀態
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <param name="paymentMethod">付款方式</param>
        /// <param name="orderStatus">訂單狀態</param>
        /// <returns></returns>
        public async Task<bool> UpdatePaymentStatusAsync(string guid, PaymentMethodEnum paymentMethod, OrderStatusEnum orderStatus)
        {
            Order entity = await _unitOfWork.Repository<Order>()
                .GetAsync(q => q.Guid == guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(guid));
            }

            entity.PaymentMethodId = (int)paymentMethod;
            entity.PaidDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();
            entity.StatusId = (int)orderStatus;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<Order>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆訂單資料
        /// </summary>
        /// <param name="updateDto">修改訂單的資料</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(OrderUpdateDto updateDto)
        {
            Order entity = await _unitOfWork.Repository<Order>()
                .GetAsync(q => q.Guid == updateDto.Guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] Order is not existed (Guid:{updateDto.Guid})");
                throw new ArgumentNullException(nameof(updateDto));
            }

            if (updateDto.PaidDate != null)
            {
                entity.PaidDate = updateDto.PaidDate.GetValueOrDefault().ToUniversalTime();
            }

            entity.Name = updateDto.Name;
            entity.Email = updateDto.Email;
            entity.Phone = updateDto.Phone;
            entity.Address = updateDto.Address;
            entity.Total = updateDto.Total;
            entity.PaymentMethodId = updateDto.PaymentMethodId;
            entity.PaidDate = updateDto.PaidDate;
            entity.StatusId = updateDto.StatusId;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<Order>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 訂單是否已經付款
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<bool> IsOrderPaidAsync(string guid)
        {
            bool result = await _unitOfWork.Repository<Order>().GetAllNoTracking()
                .AnyAsync(q => q.Guid == guid 
                            && q.StatusId > (int)OrderStatusEnum.PlaceOrder);

            return result;
        }

        /// <summary>
        /// 訂單是否存在
        /// </summary>
        /// <param name="guid">訂單GUID</param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(string guid)
        {
            bool result = await _unitOfWork.Repository<Order>().GetAllNoTracking()
                .AnyAsync(q => q.Guid == guid);

            return result;
        }
    }
}