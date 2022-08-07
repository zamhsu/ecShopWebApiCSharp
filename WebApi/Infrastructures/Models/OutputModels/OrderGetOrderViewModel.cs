using Common.Dtos;
using WebApi.Infrastructures.Models.Dtos.Orders;

namespace WebApi.Infrastructures.Models.OutputModels
{
    public class OrderGetOrderViewModel
    {
        public List<OrderDisplayDto> OrderDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}