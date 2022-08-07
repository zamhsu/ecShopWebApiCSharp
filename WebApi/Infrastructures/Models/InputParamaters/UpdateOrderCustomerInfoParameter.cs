namespace WebApi.Infrastructures.Models.InputParamaters
{
    public class UpdateOrderCustomerInfoParameter
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}