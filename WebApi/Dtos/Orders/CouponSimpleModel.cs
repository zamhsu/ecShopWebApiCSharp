namespace WebApi.Dtos.Orders
{
    public class CouponSimpleModel
    {
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int DiscountPercentage { get; set; }
    }
}