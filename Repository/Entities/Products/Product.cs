using Repository.Entities.Orders;

namespace Repository.Entities.Products
{
    public class Product
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Guid { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int UnitId { get; set; }
        public int Quantity { get; set; }
        public int OriginPrice { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDisplay { get; set; }
        public DateTimeOffset EndDisplay { get; set; }
        public string ImageUrl { get; set; }
        public string Memo { get; set; }
        public int StatusId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }

        public ProductCategoryType ProductCategoryType { get; set; }
        public ProductUnitType ProductUnitType { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}