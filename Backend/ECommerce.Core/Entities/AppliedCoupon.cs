namespace ECommerce.Core.Entities
{
    public class AppliedCoupon
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        public int CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public DateTime AppliedAt { get; set; }
        public bool IsAutoApplied { get; set; }
    }
}