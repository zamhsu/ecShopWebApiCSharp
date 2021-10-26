using System.Collections.Generic;

namespace WebApi.Models.Products
{
    public class ProductCategoryType
    {
        public ProductCategoryType()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}