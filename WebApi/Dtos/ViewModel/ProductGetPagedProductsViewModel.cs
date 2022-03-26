using WebApi.Dtos.Products;

namespace WebApi.Dtos.ViewModel
{
    public class ProductGetPagedProductsViewModel
    {
        public List<ProductDisplayModel> Products { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}