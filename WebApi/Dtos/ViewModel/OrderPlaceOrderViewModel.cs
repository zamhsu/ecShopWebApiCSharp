using WebApi.Dtos.Orders;

namespace WebApi.Dtos.ViewModel
{
    public class OrderPlaceOrderViewModel
    {
        /// <summary>
        /// 下訂單的資料
        /// </summary>
        /// <value></value>
        public PlaceOrderModel Order { get; set; } = null!;

        /// <summary>
        /// 訂單品項
        /// </summary>
        /// <value></value>
        public List<PlaceOrderDetailModel> OrderDetailModels { get; set; } = null!;

        /// <summary>
        /// 優惠碼
        /// </summary>
        /// <value></value>
        public string? CouponCode { get; set; }
    }
}