namespace WebApi.Infrastructures.Models.Dtos.Orders
{
    public class OrderDisplayDto
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
        public int? PaymentMethodId { get; set; }
        public string PaymentMethodString { get; set; }
        public DateTimeOffset? PaidDate { get; set; }
        public int StatusId { get; set; }
        public string StatusString { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}