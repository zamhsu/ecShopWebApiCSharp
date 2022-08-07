namespace WebApi.Infrastructures.Models.Dtos.Orders
{
    public class CouponSimpleDto
    {
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int DiscountPercentage { get; set; }
    }
}