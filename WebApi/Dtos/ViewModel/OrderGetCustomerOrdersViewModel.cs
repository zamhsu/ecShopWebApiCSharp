using WebApi.Dtos.Orders;

namespace WebApi.Dtos.ViewModel
{
    public class OrderGetCustomerOrdersViewModel
    {
        public List<OrderDisplayDetailModel> OrderDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}