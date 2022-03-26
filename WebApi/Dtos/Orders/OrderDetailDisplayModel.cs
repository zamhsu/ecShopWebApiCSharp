namespace WebApi.Dtos.Orders
{
    /// <summary>
    /// 訂單商品詳細資料
    /// </summary>
    public class OrderItemDetailDisplayModel
    {
        public int ItemNo { get; set; }
        public string ProductGuid { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public int Total { get; set; }
    }

    /// <summary>
    /// 訂單優惠券詳細資料
    /// </summary>
    public class OrderCouponDetailDisplayModel
    {
        public int ItemNo { get; set; }
        public string CouponCode { get; set; } = null!;
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}