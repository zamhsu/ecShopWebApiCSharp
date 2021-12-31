namespace WebApi.Dtos.Orders
{
    public class OrderDisplayModel
    {
        public string Guid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Total { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentMethodString { get; set; }
        public DateTimeOffset? PaidDate { get; set; }
        public int StatusId { get; set; }
        public string StatusString { get; set; } = null!;

        public List<OrderItemDetailDisplayModel> OrderDetails { get; set; } = null!;

        public OrderCouponDetailDisplayModel? CouponDetail { get; set; }
    }
}