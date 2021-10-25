namespace WebApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int OriginPrice { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public DateTime StartDisplay { get; set; }
        public DateTime EndDisplay { get; set; }
        public string ImageUrl { get; set; }
        public string Memo { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}