namespace WebApi.Models.Orders
{
    public class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
        public int? PaymentMethodId { get; set; }
        public DateTimeOffset? PaidDate { get; set; }
        public int StatusId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}