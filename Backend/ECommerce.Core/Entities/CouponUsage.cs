namespace ECommerce.Core.Entities
{
    public class CouponUsage
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; } = new();
        public string UserId { get; set; } = string.Empty;
        public DateTime UsedAt { get; set; }
        public int? OrderId { get; set; } // Optional: link to order if you have orders
    }
}