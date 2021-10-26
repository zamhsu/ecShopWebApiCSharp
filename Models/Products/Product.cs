namespace WebApi.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int UnitId { get; set; }
        public int Quantity { get; set; }
        public int OriginPrice { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public DateTime StartDisplay { get; set; }
        public DateTime EndDisplay { get; set; }
        public string? ImageUrl { get; set; }
        public string? Memo { get; set; }
        public int StatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public ProductCategoryType ProductCategoryType { get; set; }
        public ProductUnitType ProductUnitType { get; set; }
        public ProductStatus ProductStatus { get; set; }
    }
}