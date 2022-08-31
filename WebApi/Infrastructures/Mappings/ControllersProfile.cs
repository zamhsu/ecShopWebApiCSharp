using AutoMapper;
using Repository.Entities.Orders;
using Repository.Entities.Products;
using Service.Dtos.Members;
using Service.Dtos.Orders;
using Service.Dtos.Payments;
using WebApi.Infrastructures.Models.Dtos.Members;
using WebApi.Infrastructures.Models.Dtos.Orders;
using WebApi.Infrastructures.Models.Dtos.Payments;
using WebApi.Infrastructures.Models.Dtos.Products;
using WebApi.Infrastructures.Models.Paramaters;

namespace WebApi.Infrastructures.Mappings
{
    public class ControllersProfile : Profile
    {
        public ControllersProfile()
        {
            // AdminMember
            CreateMap<AdminMemberDetailDto, AdminMemberDisplayDto>();

            CreateMap<CreateAdminMemberParameter, AdminMemberRegisterDto>();

            CreateMap<UpdateAdminMemberInfoParameter, AdminMemberUserInfoDto>();

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
            CreateMap<CouponDetailDto, CouponDisplayDto>();

            CreateMap<CouponDto, CouponSimpleDto>();

            CreateMap<CreateCouponParameter, CouponCreateDto>()
                .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                .ForMember(dest => dest.ExpiredDate, mo => mo.Ignore());

            CreateMap<UpdateCouponParameter, CouponUpdateDto>();

            // Order
            CreateMap<Order, OrderDisplayDto>();

            CreateMap<PlaceOrderDto, Order>();

            CreateMap<UpdateOrderCustomerInfoParameter, Order>();

            // PaymentMethod
            CreateMap<PaymentMethodDto, PaymentMethodDisplayDto>();
        }
    }
}