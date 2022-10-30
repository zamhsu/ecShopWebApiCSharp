﻿namespace Service.Dtos.Products
{
    public class ProductUpdateDto
    {
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
    }
}
