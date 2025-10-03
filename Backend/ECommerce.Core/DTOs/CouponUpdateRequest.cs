namespace ECommerce.Core.DTOs
{
    public class CouponUpdateRequest
    {
        public string? Description { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public bool? IsAutoApplied { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? MinimumCartItems { get; set; }
        public decimal? MinimumTotalPrice { get; set; }
        public int? MaxTotalUses { get; set; }
        public int? MaxUsesPerUser { get; set; }
        public List<int>? ApplicableProductIds { get; set; }
        public bool? IsActive { get; set; }
    }
}
