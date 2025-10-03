using ECommerce.Core.Entities;

namespace ECommerce.Core.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartByIdAsync(int cartId);
        Task<Cart> CreateCartAsync(int userId);
        Task<Cart> UpdateCartAsync(Cart cart);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
        Task<List<AppliedCoupon>> GetAppliedCouponsAsync(int cartId);
        Task<AppliedCoupon> AddAppliedCouponAsync(AppliedCoupon appliedCoupon);
        Task RemoveAppliedCouponAsync(int cartId, string couponCode);
        Task ClearCartAsync(int cartId);
    }
}
