namespace Repository.Entities.Orders
{
    public class CouponStatus
    {
        public CouponStatus()
        {
            Coupon = new HashSet<Coupon>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Coupon> Coupon { get; set; }
    }
}