using Common.Dtos;
using WebApi.Infrastructures.Models.Dtos.Orders;

namespace WebApi.Infrastructures.Models.OutputModels
{
    public class CouponGetCouponViewModel
    {
        public List<CouponDisplayDto> CouponDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}