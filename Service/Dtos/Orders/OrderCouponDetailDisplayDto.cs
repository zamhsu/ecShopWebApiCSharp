using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Orders
{
    /// <summary>
    /// 訂單優惠券詳細資料
    /// </summary>
    public class OrderCouponDetailDisplayDto
    {
        public int ItemNo { get; set; }
        public string CouponCode { get; set; } = null!;
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
