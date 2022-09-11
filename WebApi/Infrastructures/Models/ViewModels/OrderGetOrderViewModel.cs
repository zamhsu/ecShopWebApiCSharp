using Common.Dtos;
using WebApi.Infrastructures.Models.Dtos.Orders;

namespace WebApi.Infrastructures.Models.ViewModels
{
    public class OrderGetOrderViewModel
    {
        public List<OrderDisplayDto> OrderDisplays { get; set; }

        public Pagination Pagination { get; set; }
    }
}