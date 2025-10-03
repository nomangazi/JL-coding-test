namespace ECommerce.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; } = null;

        // Navigation properties
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
    }
}