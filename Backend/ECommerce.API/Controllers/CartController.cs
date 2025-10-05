using ECommerce.Core.DTOs;
using ECommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        // GET: api/cart/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartResponse>> GetCart(int userId)
        {
            try
            {
                var cart = await _cartService.GetCartAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart for user {UserId}", userId);
                return StatusCode(500, new { message = "An error occurred while retrieving the cart" });
            }
        }

        // POST: api/cart/{userId}/items
        [HttpPost("{userId}/items")]
        public async Task<ActionResult<CartResponse>> AddItemToCart([FromRoute] int userId, [FromBody] AddCartItemRequest request)
        {
            try
            {
                var cart = await _cartService.AddItemToCartAsync(userId, request);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/cart/{userId}/items/{productId}
        [HttpPut("{userId}/items/{productId}")]
        public async Task<ActionResult<CartResponse>> UpdateCartItem(
            int userId,
            int productId,
            [FromBody] UpdateCartItemRequest request)
        {
            try
            {
                var cart = await _cartService.UpdateCartItemAsync(userId, productId, request);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item for user {UserId}", userId);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/cart/{userId}/items/{productId}
        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult<CartResponse>> RemoveItemFromCart(int userId, int productId)
        {
            try
            {
                var cart = await _cartService.RemoveItemFromCartAsync(userId, productId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart for user {UserId}", userId);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/cart/{userId}/coupons
        [HttpPost("{userId}/coupons")]
        public async Task<ActionResult<CartResponse>> ApplyCoupon([FromRoute] int userId, [FromBody] ApplyCouponRequest request)
        {
            try
            {
                // validate request
                request.Validate();
                var cart = await _cartService.ApplyCouponAsync(userId, request.CouponCode);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying coupon for user {UserId}", userId);
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/cart/{userId}/coupons/{couponCode}
        [HttpDelete("{userId}/coupons/{couponCode}")]
        public async Task<ActionResult<CartResponse>> RemoveCoupon(int userId, string couponCode)
        {
            try
            {
                var cart = await _cartService.RemoveCouponAsync(userId, couponCode);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing coupon for user {UserId}", userId);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/cart/{userId}/apply-auto-coupons
        [HttpPost("{userId}/apply-auto-coupons")]
        public async Task<ActionResult<CartResponse>> ApplyAutoCoupons(int userId)
        {
            try
            {
                var cart = await _cartService.ApplyAutoAppliedCouponsAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying auto coupons for user {UserId}", userId);
                return StatusCode(500, new { message = "An error occurred while applying auto coupons" });
            }
        }

        // DELETE: api/cart/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            try
            {
                await _cartService.ClearCartAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
                return StatusCode(500, new { message = "An error occurred while clearing the cart" });
            }
        }
    }
}
