using AutoMapper;
using WebApi.Dtos.Members;
using WebApi.Dtos.Orders;
using WebApi.Models.Members;
using WebApi.Models.Orders;

namespace WebApi.Base.Mappings
{
    public class ServicesProfile : Profile
    {
        public ServicesProfile()
        {
            // AdminMember
            CreateMap<AdminMember, AdminMemberInfoModel>();

            // Order
            CreateMap<Order, OrderDisplayModel>()
                .ForMember(dest => dest.PaymentMethodString, mo => mo.MapFrom(q => q.PaymentMethod.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.OrderStatus.Name));

            // OrderDetail
            CreateMap<OrderDetail, OrderItemDetailDisplayModel>()
                .ForMember(dest => dest.ProductGuid, mo => mo.MapFrom(q => q.Product.Guid))
                .ForMember(dest => dest.ProductName, mo => mo.MapFrom(q => q.Product.Title));

            CreateMap<OrderDetail, OrderCouponDetailDisplayModel>()
                .ForMember(dest => dest.CouponCode, mo => mo.MapFrom(q => q.Coupon.Code));
        }
    }
}