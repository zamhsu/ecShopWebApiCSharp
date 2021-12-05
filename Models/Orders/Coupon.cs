namespace WebApi.Models.Orders
{
    public class Coupon
    {
        public Coupon()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public int Quantity { get; set; }
        public int DiscountPercentage { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}