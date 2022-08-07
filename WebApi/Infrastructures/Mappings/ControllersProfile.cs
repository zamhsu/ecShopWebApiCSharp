using AutoMapper;
using Repository.Entities.Members;
using Repository.Entities.Orders;
using Repository.Entities.Products;
using WebApi.Infrastructures.Models.Dtos.Members;
using WebApi.Infrastructures.Models.Dtos.Orders;
using WebApi.Infrastructures.Models.Dtos.Payments;
using WebApi.Infrastructures.Models.Dtos.Products;
using WebApi.Infrastructures.Models.InputParamaters;

namespace WebApi.Infrastructures.Mappings
{
    public class ControllersProfile : Profile
    {
        public ControllersProfile()
        {
            // AdminMember
            CreateMap<AdminMember, AdminMemberDisplayDto>()
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.AdminMemberStatus.Name));

            CreateMap<CreateAdminMemberParameter, AdminMember>();

            CreateMap<UpdateAdminMemberInfoParameter, AdminMember>();

            // Product
            CreateMap<Product, ProductDisplayDto>()
                .ForMember(dest => dest.CategoryString, mo => mo.MapFrom(q => q.ProductCategoryType.Name))
                .ForMember(dest => dest.UnitString, mo => mo.MapFrom(q => q.ProductUnitType.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.ProductStatus.Name));

            CreateMap<CreateProductParameter, Product>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());

            CreateMap<UpdateProductParameter, Product>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());

            // ProductCategoryType
            CreateMap<ProductCategoryType, ProductCategoryTypeDisplayDto>();

            CreateMap<CreateProductCategoryTypeParameter, ProductCategoryType>();

            CreateMap<UpdateProductCategoryTypeParameter, ProductCategoryType>();

            // ProductUnitType
            CreateMap<ProductUnitType, ProductUnitTypeDisplayDto>();

            CreateMap<CreateProductUnitTypeParameter, ProductUnitType>();

            CreateMap<UpdateProductUnitTypeParameter, ProductUnitType>();

            // Coupon
            CreateMap<Coupon, CouponDisplayDto>()
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.CouponStatus.Name));

            CreateMap<Coupon, CouponSimpleDto>();

            CreateMap<CreateCouponParameter, Coupon>()
                .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                .ForMember(dest => dest.ExpiredDate, mo => mo.Ignore());

            CreateMap<UpdateCouponParameter, Coupon>()
                .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                .ForMember(dest => dest.ExpiredDate, mo => mo.Ignore());

            // Order
            CreateMap<Order, OrderDisplayDto>();

            CreateMap<PlaceOrderDto, Order>();

            CreateMap<UpdateOrderCustomerInfoParameter, Order>();

            // PaymentMethod
            CreateMap<PaymentMethod, PaymentMethodDisplayDto>();
        }
    }
}