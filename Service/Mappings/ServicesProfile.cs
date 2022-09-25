using AutoMapper;
using Repository.Entities.Members;
using Repository.Entities.Orders;
using Repository.Entities.Products;
using Service.Dtos.Members;
using Service.Dtos.Orders;
using Service.Dtos.Payments;
using Service.Dtos.Products;

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
            CreateMap<AdminMemberCreateDto, AdminMember>();

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

            // ProductCategoryType
            CreateMap<ProductCategoryType, ProductCategoryTypeDto>();
            CreateMap<ProductCategoryTypeCreateDto, ProductCategoryType>();

            // ProductUnitType
            CreateMap<ProductUnitType, ProductUnitTypeDto>();
            CreateMap<ProductUnitTypeCreateDto, ProductUnitType>();

            // Product
            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.UnitString, mo => mo.MapFrom(q => q.ProductUnitType.Name))
                .ForMember(dest => dest.CategoryString, mo => mo.MapFrom(q => q.ProductCategoryType.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.ProductStatus.Name));
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}