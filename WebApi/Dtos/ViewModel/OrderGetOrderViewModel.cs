using WebApi.Dtos.Members;
using WebApi.Dtos.Orders;

namespace WebApi.Dtos.ViewModel
{
    public class OrderGetOrderViewModel
    {
        public List<OrderDisplayModel> OrderDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}