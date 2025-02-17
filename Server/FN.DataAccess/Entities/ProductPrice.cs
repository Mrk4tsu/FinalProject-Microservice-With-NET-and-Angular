using FN.DataAccess.Enums;

namespace FN.DataAccess.Entities
{
    public class ProductPrice
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public PriceType PriceType { get; set; } = PriceType.BASE;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ProductDetail Product { get; set; } = new ProductDetail();
    }
}
