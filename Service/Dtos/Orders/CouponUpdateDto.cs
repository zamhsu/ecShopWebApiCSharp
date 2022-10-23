namespace Service.Dtos.Orders
{
    public class CouponUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Used { get; set; }
        public int StatusId { get; set; }
    }
}
