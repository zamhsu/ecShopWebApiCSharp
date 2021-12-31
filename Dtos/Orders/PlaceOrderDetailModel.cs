namespace WebApi.Dtos.Orders
{
    public class PlaceOrderDetailModel
    {
        public int ProductId { get; set; }
        public string ProductGuid { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}