using AutoMapper;
using WebApi.Dtos.Products;
using WebApi.Models.Products;

namespace WebApi.Base.Mappings
{
    public class ServicesProfile : Profile
    {
        public ServicesProfile()
        {
            // Product
            CreateMap<CreateProductModel, Product>()
                .ForMember(dest => dest.Guid, mo => mo.Ignore())
                .ForMember(dest => dest.StartDisplay, mo => mo.Ignore())
                .ForMember(dest => dest.EndDisplay, mo => mo.Ignore());

            // ProductCategoryType
            CreateMap<CreateProductCategoryTypeModel, ProductCategoryType>();

            // ProductUnitType
            CreateMap<CreateProductUnitTypeModel, ProductUnitType>();
        }
    }
}