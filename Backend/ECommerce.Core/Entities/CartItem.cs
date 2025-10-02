namespace ECommerce.Core.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}