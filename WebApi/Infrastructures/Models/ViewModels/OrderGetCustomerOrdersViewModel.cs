using Common.Dtos;
using Service.Dtos.Orders;

namespace WebApi.Infrastructures.Models.ViewModels
{
    public class OrderGetCustomerOrdersViewModel
    {
        public List<OrderDisplayDetailDto> OrderDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}