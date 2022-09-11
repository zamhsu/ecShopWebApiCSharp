namespace WebApi.Infrastructures.Models.Dtos.Orders
{
    public class CouponDisplayDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public int Quantity { get; set; }
        public int Used { get; set; }
        public int DiscountPercentage { get; set; }
        public int StatusId { get; set; }
        public string StatusString { get; set; }
    }
}