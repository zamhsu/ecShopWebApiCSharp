namespace WebApi.Infrastructures.Models.InputParamaters
{
    public class CustomerPaymentParameter
    {
        /// <summary>
        /// 訂單Guid
        /// </summary>
        /// <value></value>
        public string OrderGuid { get; set; } = null!;

        /// <summary>
        /// 付款方式Id
        /// </summary>
        /// <value></value>
        public int PaymentMethodId { get; set; }
    }
}