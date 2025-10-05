namespace ECommerce.Core.DTOs
{
    public class CouponValidationRequest
    {
        public string Code { get; set; } = string.Empty;
        public int UserId { get; set; }
        public decimal CartTotal { get; set; }
        public int ItemCount { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                throw new Exception("Coupon code is required");
            }
            if (UserId <= 0)
            {
                throw new Exception("Valid UserId is required");
            }
            if (CartTotal < 0)
            {
                throw new Exception("CartTotal cannot be negative");
            }
            if (ItemCount < 0)
            {
                throw new Exception("ItemCount cannot be negative");
            }
            return true;
        }
    }
}
