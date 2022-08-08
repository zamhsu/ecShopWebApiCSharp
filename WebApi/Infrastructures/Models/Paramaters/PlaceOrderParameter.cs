using Service.Dtos.Orders;
using WebApi.Infrastructures.Models.Dtos.Orders;

namespace WebApi.Infrastructures.Models.Paramaters
{
    public class PlaceOrderParameter
    {
        /// <summary>
        /// 下訂單的資料
        /// </summary>
        /// <value></value>
        public PlaceOrderDto Order { get; set; } = null!;

        /// <summary>
        /// 訂單品項
        /// </summary>
        /// <value></value>
        public List<PlaceOrderDetailDto> OrderDetailModels { get; set; } = null!;

        /// <summary>
        /// 優惠碼
        /// </summary>
        /// <value></value>
        public string CouponCode { get; set; }
    }
}