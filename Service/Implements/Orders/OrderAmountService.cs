using Repository.Entities.Orders;
using Service.Dtos.Orders;
using Service.Interfaces.Orders;

namespace Service.Implements.Orders
{
    public class OrderAmountService : IOrderAmountService
    {
        /// <summary>
        /// 計算商品金額
        /// </summary>
        /// <param name="item">商品</param>
        /// <returns></returns>
        public int CalculateItemTotal(PlaceOrderDetailDto item)
        {
            int total = item.Price * item.Quantity;

            return total;
        }

        /// <summary>
        /// 計算折扣金額(負數)
        /// </summary>
        /// <param name="coupon">優惠券</param>
        /// <param name="itemTotalAmount">商品總金額</param>
        /// <returns></returns>
        public int CalculateDiscountAmount(Coupon coupon, int itemTotalAmount)
        {
            decimal percent = Convert.ToDecimal(coupon.DiscountPercentage) / 100M;
            // 折扣後金額
            decimal afterDiscountAmount = Convert.ToDecimal(itemTotalAmount) * percent;
            // 折扣金額(負數)
            int discountAmount = (itemTotalAmount - decimal.ToInt32(afterDiscountAmount)) * -1;

            return discountAmount;
        }

        /// <summary>
        /// 計算應付金額
        /// </summary>
        /// <param name="itemTotalAmount">商品總金額</param>
        /// <param name="discountAmount">優惠金額(負數)</param>
        /// <returns></returns>
        public int CalculateTotalAmount(int itemTotalAmount, int discountAmount)
        {
            if (discountAmount > 0)
            {
                throw new ArgumentException("優惠金額應該是負數");
            }
            
            int totalAmount = itemTotalAmount + discountAmount;

            return totalAmount;
        }
    }
}