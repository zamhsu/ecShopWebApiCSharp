using AutoMapper;
using Service.Dtos.Members;
using Service.Dtos.Orders;
using Service.Dtos.Payments;
using Service.Dtos.Products;
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
            CreateMap<ProductDto, ProductDisplayDto>();
            CreateMap<ProductDetailDto, ProductDisplayDto>();
            CreateMap<CreateProductParameter, ProductCreateDto>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());
            CreateMap<UpdateProductParameter, ProductUpdateDto>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());

            // ProductCategoryType
            CreateMap<ProductCategoryTypeDto, ProductCategoryTypeDisplayDto>();
            CreateMap<CreateProductCategoryTypeParameter, ProductCategoryTypeCreateDto>();
            CreateMap<UpdateProductCategoryTypeParameter, ProductCategoryTypeUpdateDto>();

            // ProductUnitType
            CreateMap<ProductUnitTypeDto, ProductUnitTypeDisplayDto>();
            CreateMap<CreateProductUnitTypeParameter, ProductUnitTypeCreateDto>();
            CreateMap<UpdateProductUnitTypeParameter, ProductUnitTypeUpdateDto>();

            // Coupon
            CreateMap<CouponDetailDto, CouponDisplayDto>();
            CreateMap<CouponDto, CouponSimpleDto>();
            CreateMap<CreateCouponParameter, CouponCreateDto>()
                .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                .ForMember(dest => dest.ExpiredDate, mo => mo.Ignore());
            CreateMap<UpdateCouponParameter, CouponUpdateDto>();

            // Order
            CreateMap<OrderDetailDto, OrderDisplayDto>();
            CreateMap<PlaceOrderDto, OrderCustomerInfoDto>();
            CreateMap<UpdateOrderCustomerInfoParameter, OrderCustomerInfoDto>();

            // PaymentMethod
            CreateMap<PaymentMethodDto, PaymentMethodDisplayDto>();
        }
    }
}