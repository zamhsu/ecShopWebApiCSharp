using WebApi.Models.Products;

namespace WebApi.Models.Orders
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ItemNo { get; set; }
        public int? ProductId { get; set; }
        public int? CouponId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }

        public Order Order { get; set; }
        public Product? Product { get; set; }
        public Coupon? Coupon { get; set; }
    }
}