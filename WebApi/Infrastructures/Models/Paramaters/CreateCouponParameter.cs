namespace WebApi.Infrastructures.Models.Paramaters
{
    public class CreateCouponParameter
    {
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int Quantity { get; set; }
        public int Used { get; set; }
        public int DiscountPercentage { get; set; }
        public int StatusId { get; set; }
    }
}