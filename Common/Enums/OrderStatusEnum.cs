namespace Common.Enums
{
    /// <summary>
    /// 訂單狀態參數
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// 建立訂單
        /// </summary>
        PlaceOrder = 1,
        /// <summary>
        /// 完成付款
        /// </summary>
        PaymentSuccessful = 2,
        /// <summary>
        /// 付款失敗
        /// </summary>
        PaymentFailed = 3
    }
}
