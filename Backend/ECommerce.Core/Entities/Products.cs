namespace ECommerce.Core.Entities
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public decimal PriceOffice { get; set; } = 0;
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
    }
}