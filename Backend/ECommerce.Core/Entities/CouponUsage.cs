namespace ECommerce.Core.Entities
{
    public class CouponUsage
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime UsedAt { get; set; }
        public int? OrderId { get; set; } // Optional: link to order if you have orders
    }
}