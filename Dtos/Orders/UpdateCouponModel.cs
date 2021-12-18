namespace WebApi.Dtos.Orders
{
    public class UpdateCouponModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Used { get; set; }
        public int StatusId { get; set; }
    }
}