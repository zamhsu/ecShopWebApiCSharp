using AutoMapper;
using WebApi.Dtos.Products;
using WebApi.Models.Products;

namespace WebApi.Base.Mappings
{
    public class ControllersProfile : Profile
    {
        public ControllersProfile()
        {
            // Product
            CreateMap<Product, ProductDisplayModel>()
                .ForMember(dest => dest.CategoryString, mo => mo.MapFrom(q=>q.ProductCategoryType.Name))
                .ForMember(dest => dest.UnitString, mo => mo.MapFrom(q=>q.ProductUnitType.Name))
                .ForMember(dest => dest.StatusString, mo => mo.MapFrom(q=>q.ProductStatus.Name));

            CreateMap<CreateProductModel, Product>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());

            CreateMap<UpdateProductModel, Product>()
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());
        }
    }
}