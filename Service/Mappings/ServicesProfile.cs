using AutoMapper;
using Repository.Entities.Members;
using Repository.Entities.Orders;
using Service.Dtos.Members;
using Service.Dtos.Orders;
using Service.Dtos.Payments;

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

            // Coupon
            CreateMap<Coupon, CouponDto>();
            CreateMap<Coupon, CouponDetailDto>()
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.CouponStatus.Name));
            CreateMap<CouponCreateDto, Coupon>();

            // Order
            CreateMap<Order, OrderDto>();
            CreateMap<Order, OrderDetailDto>()
                .ForMember(dest => dest.PaymentMethodString, mo => mo.MapFrom(q => q.PaymentMethod.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.OrderStatus.Name));
            CreateMap<Order, OrderDisplayDetailDto>()
                .ForMember(dest => dest.PaymentMethodString, mo => mo.MapFrom(q => q.PaymentMethod.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.OrderStatus.Name));
            CreateMap<OrderCustomerInfoDto, Order>();

            // OrderDetail
            CreateMap<OrderDetail, OrderItemDetailDisplayDto>()
                .ForMember(dest => dest.ProductGuid, mo => mo.MapFrom(q => q.Product.Guid))
                .ForMember(dest => dest.ProductName, mo => mo.MapFrom(q => q.Product.Title));

            CreateMap<OrderDetail, OrderCouponDetailDisplayDto>()
                .ForMember(dest => dest.CouponCode, mo => mo.MapFrom(q => q.Coupon.Code));

            // Payment
            CreateMap<PaymentMethod, PaymentMethodDto>();
        }
    }
}