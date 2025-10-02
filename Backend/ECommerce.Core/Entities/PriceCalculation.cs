namespace ECommerce.Core.Entities
{
    class PriceCalculation
    {
        public decimal TotalBeforeDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal FinalPayableAmount { get; set; }
        public List<AppliedDiscountDetail> DiscountDetails { get; set; } = new();
    }
    public class AppliedDiscountDetail
    {
        public string CouponCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}