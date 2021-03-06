using AutoMapper;
using WebApi.Dtos.Members;
using WebApi.Dtos.Orders;
using WebApi.Dtos.Payments;
using WebApi.Dtos.Products;
using WebApi.Models.Members;
using WebApi.Models.Orders;
using WebApi.Models.Products;

namespace WebApi.Base.Mappings
{
    public class ControllersProfile : Profile
    {
        public ControllersProfile()
        {
            // AdminMember
            CreateMap<AdminMember, AdminMemberDisplayModel>()
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.AdminMemberStatus.Name));

            CreateMap<CreateAdminMemberModel, AdminMember>();

            CreateMap<UpdateAdminMemberInfoModel, AdminMember>();

            // Product
            CreateMap<Product, ProductDisplayModel>()
                .ForMember(dest => dest.CategoryString, mo => mo.MapFrom(q => q.ProductCategoryType.Name))
                .ForMember(dest => dest.UnitString, mo => mo.MapFrom(q => q.ProductUnitType.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.ProductStatus.Name));

            CreateMap<CreateProductModel, Product>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());

            CreateMap<UpdateProductModel, Product>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());

            // ProductCategoryType
            CreateMap<ProductCategoryType, ProductCategoryTypeDisplayModel>();

            CreateMap<CreateProductCategoryTypeModel, ProductCategoryType>();

            CreateMap<UpdateProductCategoryTypeModel, ProductCategoryType>();

            // ProductUnitType
            CreateMap<ProductUnitType, ProductUnitTypeDisplayModel>();

            CreateMap<CreateProductUnitTypeModel, ProductUnitType>();

            CreateMap<UpdateProductUnitTypeModel, ProductUnitType>();

            // Coupon
            CreateMap<Coupon, CouponDisplayModel>()
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q => q.CouponStatus.Name));

            CreateMap<Coupon, CouponSimpleModel>();

            CreateMap<CreateCouponModel, Coupon>()
                .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                .ForMember(dest => dest.ExpiredDate, mo => mo.Ignore());

            CreateMap<UpdateCouponModel, Coupon>()
                .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                .ForMember(dest => dest.ExpiredDate, mo => mo.Ignore());

            // Order
            CreateMap<PlaceOrderModel, Order>();

            CreateMap<UpdateOrderCustomerInfoModel, Order>();

            // PaymentMethod
            CreateMap<PaymentMethod, PaymentMethodDisplayModel>();
        }
    }
}