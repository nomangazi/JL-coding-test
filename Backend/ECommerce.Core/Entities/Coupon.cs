using Newtonsoft.Json;

namespace ECommerce.Core.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Discount settings
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }

        // Application settings
        public bool IsAutoApplied { get; set; }

        // Time restrictions
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Cart restrictions
        public int? MinimumCartItems { get; set; }
        public decimal? MinimumTotalPrice { get; set; }

        // Usage restrictions
        public int? MaxTotalUses { get; set; }
        public int? MaxUsesPerUser { get; set; }
        public int CurrentTotalUses { get; set; }

        // Product restrictions
        public string ApplicableProductIdsJson { get; set; } = string.Empty; // JSON serialized list of product IDs

        public bool IsActive { get; set; }

        // Navigation properties
        public List<AppliedCoupon> AppliedCoupons { get; set; } = new List<AppliedCoupon>(); // Coupons applied to carts
        public List<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; } = null;

        // Helper methods
        public List<int> GetApplicableProductIds()
        {
            if (string.IsNullOrEmpty(ApplicableProductIdsJson))
            {
                return new List<int>();
            }

            return JsonConvert.DeserializeObject<List<int>>(ApplicableProductIdsJson) ?? new List<int>();
        }

        public void SetApplicableProductIds(List<int> productIds)
        {
            // ApplicableProductIdsJson = System.Text.Json.JsonSerializer.Serialize(productIds);
            ApplicableProductIdsJson = JsonConvert.SerializeObject(productIds);
        }
    }

    public enum DiscountType
    {
        Fixed = 1,
        Percentage = 2
    }
}