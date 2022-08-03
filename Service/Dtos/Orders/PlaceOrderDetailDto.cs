namespace Service.Dtos.Orders
{
    public class PlaceOrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductGuid { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}