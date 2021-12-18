using WebApi.Base.IServices.Orders;
using WebApi.Dtos.Orders;
using WebApi.Models.Orders;

namespace WebApi.Base.Services.Orders
{
    public class OrderAmountService : IOrderAmountService
    {
        /// <summary>
        /// 計算商品金額
        /// </summary>
        /// <param name="item">商品</param>
        /// <returns></returns>
        public int CalculateItemTotal(PlaceOrderDetailModel item)
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
            // 折扣後金額
            int afterDiscountAmount = itemTotalAmount * (coupon.DiscountPercentage / 100);
            // 折扣金額(負數)
            int discountAmount = (itemTotalAmount - afterDiscountAmount) * -1;

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
            int totalAmount = itemTotalAmount + discountAmount;

            return totalAmount;
        }
    }
}