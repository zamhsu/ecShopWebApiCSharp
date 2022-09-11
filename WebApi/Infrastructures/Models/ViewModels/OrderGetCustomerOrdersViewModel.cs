using Common.Dtos;
using Service.Dtos.Orders;

namespace WebApi.Infrastructures.Models.ViewModels
{
    public class OrderGetCustomerOrdersViewModel
    {
        public List<OrderDisplayDetailDto> OrderDisplays { get; set; }

        public Pagination Pagination { get; set; }
    }
}