using WebApi.Dtos.Members;
using WebApi.Dtos.Orders;

namespace WebApi.Dtos.ViewModel
{
    public class CouponGetCouponViewModel
    {
        public List<CouponDisplayModel> CouponDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}