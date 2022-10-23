namespace Service.Dtos.Orders
{
    /// <summary>
    /// 訂單商品詳細資料
    /// </summary>
    public class OrderItemDetailDisplayDto
    {
        public int ItemNo { get; set; }
        public string ProductGuid { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
