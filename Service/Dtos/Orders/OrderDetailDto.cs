namespace Service.Dtos.Orders
{
    public class OrderDetailDto : OrderDto
    {
        public string StatusString { get; set; }
        public string PaymentMethodString { get; set; }
    }
}
