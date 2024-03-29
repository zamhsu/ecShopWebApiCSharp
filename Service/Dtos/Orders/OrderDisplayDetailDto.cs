namespace Service.Dtos.Orders
{
    public class OrderDisplayDetailDto
    {
        public string Guid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Total { get; set; }
        public int? PaymentMethodId { get; set; }
        public string PaymentMethodString { get; set; }
        public DateTimeOffset? PaidDate { get; set; }
        public int StatusId { get; set; }
        public string StatusString { get; set; } = null!;
        public DateTimeOffset CreateDate { get; set; }

        public List<OrderItemDetailDisplayDto> OrderDetails { get; set; } = null!;

        public OrderCouponDetailDisplayDto CouponDetail { get; set; }
    }
}