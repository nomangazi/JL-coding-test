using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        // GET: api/coupon
        [HttpGet]
        public async Task<ActionResult<List<Coupon>>> GetAllCoupons()
        {
            try
            {
                var coupons = await _couponService.GetAllCouponsAsync();
                return Ok(coupons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/coupon/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Coupon>> GetCouponById(int id)
        {
            try
            {
                var coupon = await _couponService.GetCouponByIdAsync(id);
                if (coupon == null)
                {
                    return NotFound(new { message = "Coupon not found" });
                }
                return Ok(coupon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/coupon/by-code/{code}
        [HttpGet("by-code/{code}")]
        public async Task<ActionResult<Coupon>> GetCouponByCode(string code)
        {
            try
            {
                var coupon = await _couponService.GetCouponByCodeAsync(code);
                if (coupon == null)
                {
                    return NotFound(new { message = "Coupon not found" });
                }
                return Ok(coupon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/coupon
        [HttpPost]
        public async Task<ActionResult<Coupon>> CreateCoupon([FromBody] CouponCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var coupon = await _couponService.CreateCouponAsync(request);
                return CreatedAtAction(nameof(GetCouponById), new { id = coupon.Id }, coupon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/coupon/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Coupon>> UpdateCoupon(int id, [FromBody] CouponUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var coupon = await _couponService.UpdateCouponAsync(id, request);
                return Ok(coupon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/coupon/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCoupon(int id)
        {
            try
            {
                await _couponService.DeleteCouponAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/coupon/validate
        [HttpPost("validate")]
        public async Task<ActionResult<CouponValidationResult>> ValidateCoupon([FromBody] CouponValidationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Note: This is a simplified validation - in a real app you'd get the cart from the database
                // For now, we'll create a mock cart based on the request data
                var cart = new Cart
                {
                    UserId = request.UserId,
                    Items = new List<CartItem>() // This would be populated from the database
                };

                var result = await _couponService.ValidateCouponAsync(request.Code, request.UserId, cart);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}