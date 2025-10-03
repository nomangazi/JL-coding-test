namespace ECommerce.Core.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public User User { get; set; } = new();
        public List<CartItem> Items { get; set; } = [];
        public List<AppliedCoupon> AppliedCoupons { get; set; } = [];

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Computed properties
        public decimal TotalBeforeDiscount => Items.Sum(i => i.Subtotal);
        public int TotalItems => Items.Sum(i => i.Quantity);
    }

}