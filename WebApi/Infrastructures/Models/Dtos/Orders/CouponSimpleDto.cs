namespace WebApi.Infrastructures.Models.Dtos.Orders
{
    public class CouponSimpleDto
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int DiscountPercentage { get; set; }
    }
}