using AutoMapper;
using Repository.Entities.Members;
using Repository.Entities.Orders;
using Service.Dtos.Members;
using Service.Dtos.Orders;

namespace Service.Mappings
{
    public class ServicesProfile : Profile
    {
        public ServicesProfile()
        {
            // AdminMember
            CreateMap<AdminMember, AdminMemberInfoDto>();
            CreateMap<AdminMember, AdminMemberDto>();
            CreateMap<AdminMember, AdminMemberDetailDto>()
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.AdminMemberStatus.Name));
            CreateMap<AdminMemberDto, AdminMemberInfoDto>();
            CreateMap<AdminMemberDto, AdminMemberUpdateDto>();

            // Order
            CreateMap<Order, OrderDisplayDetailDto>()
                .ForMember(dest => dest.PaymentMethodString, mo => mo.MapFrom(q => q.PaymentMethod.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.OrderStatus.Name));

            // OrderDetail
            CreateMap<OrderDetail, OrderItemDetailDisplayDto>()
                .ForMember(dest => dest.ProductGuid, mo => mo.MapFrom(q => q.Product.Guid))
                .ForMember(dest => dest.ProductName, mo => mo.MapFrom(q => q.Product.Title));

            CreateMap<OrderDetail, OrderCouponDetailDisplayDto>()
                .ForMember(dest => dest.CouponCode, mo => mo.MapFrom(q => q.Coupon.Code));
        }
    }
}