namespace Service.Dtos.Orders
{
    public class CouponCreateDto
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public int Quantity { get; set; }
        public int Used { get; set; }
        public int DiscountPercentage { get; set; }
        public int StatusId { get; set; }
    }
}
