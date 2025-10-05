namespace ECommerce.Core.DTOs
{
    public class ApplyCouponRequest
    {
        public string CouponCode { get; set; } = string.Empty;

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(CouponCode))
            {
                throw new Exception("Coupon code is required");
            }
            return true;
        }
    }
}