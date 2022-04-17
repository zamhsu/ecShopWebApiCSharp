namespace WebApi.Dtos.Orders
{
    public class CustomerOrderQueryParamModel
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;
    }
}