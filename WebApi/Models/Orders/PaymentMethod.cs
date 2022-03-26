namespace WebApi.Models.Orders
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}