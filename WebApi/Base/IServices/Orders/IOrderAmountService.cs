using WebApi.Dtos.Orders;
using WebApi.Models.Orders;

namespace WebApi.Base.IServices.Orders
{
    public interface IOrderAmountService
    {
        /// <summary>
        /// 計算商品金額
        /// </summary>
        /// <param name="item">商品</param>
        /// <returns></returns>
        int CalculateItemTotal(PlaceOrderDetailModel item);

        /// <summary>
        /// 計算折扣金額(負數)
        /// </summary>
        /// <param name="coupon">優惠券</param>
        /// <param name="itemTotalAmount">商品總金額</param>
        /// <returns></returns>
        int CalculateDiscountAmount(Coupon coupon, int itemTotalAmount);

        /// <summary>
        /// 計算應付金額
        /// </summary>
        /// <param name="itemTotalAmount">商品總金額</param>
        /// <param name="discountAmount">優惠金額(負數)</param>
        /// <returns></returns>
        int CalculateTotalAmount(int itemTotalAmount, int discountAmount);
    }
}