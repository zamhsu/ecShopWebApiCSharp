using Common.Dtos;
using WebApi.Infrastructures.Models.Dtos.Products;

namespace WebApi.Infrastructures.Models.ViewModels
{
    public class ProductGetPagedProductsViewModel
    {
        public List<ProductDisplayDto> Products { get; set; }

        public Pagination Pagination { get; set; }
    }
}