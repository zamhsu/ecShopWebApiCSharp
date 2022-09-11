using System.Collections.Generic;

namespace Repository.Entities.Products
{
    public class ProductUnitType
    {
        public ProductUnitType()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}