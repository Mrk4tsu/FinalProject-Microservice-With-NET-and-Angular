using FN.DataAccess.Enums;

namespace FN.DataAccess.Entities
{
    public class ProductPrice
    {
        public int Id { get; set; }
        public int ProductDetailId { get; set; }
        public decimal Price { get; set; }
        public PriceType PriceType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProductDetail ProductDetail { get; set; }
    }
}
