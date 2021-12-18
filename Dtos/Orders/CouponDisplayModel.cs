namespace WebApi.Dtos.Orders
{
    public class CouponDisplayModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public int Quantity { get; set; }
        public int Used { get; set; }
        public int DiscountPercentage { get; set; }
        public int StatusId { get; set; }
        public string? StatusString { get; set; }
    }
}