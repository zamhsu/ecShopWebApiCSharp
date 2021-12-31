namespace WebApi.Dtos.Orders
{
    public class UpdateOrderCustomerInfoModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}