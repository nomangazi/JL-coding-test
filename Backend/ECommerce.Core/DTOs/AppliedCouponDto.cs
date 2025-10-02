using ECommerce.Core.Entities;

namespace ECommerce.Core.DTOs
{
    public class AppliedCouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAutoApplied { get; set; }
        public DateTime AppliedAt { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
    }
}