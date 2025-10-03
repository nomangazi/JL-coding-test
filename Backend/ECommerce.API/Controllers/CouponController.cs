using ECommerce.Core.DTOs;
using ECommerce.Core.DTOs;

using ECommerce.Core.Entities;
using ECommerce.Core.Services;

using ECommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

namespace ECommerce.API.Controllers;

[ApiController]

[ApiController]
[Route("api/[controller]")]

[Route("api/[controller]")]
public class CouponController : ControllerBase

public class CouponController : ControllerBase
{

{    private readonly ICouponService _couponService;

    private readonly ICouponService _couponService;

    private readonly ILogger<CouponController> _logger; public CouponController(ICouponService couponService)

    {

    public CouponController(ICouponService couponService, ILogger<CouponController> logger)        _couponService = couponService;

    {    }

        _couponService = couponService;

        _logger = logger;    // POST: api/coupon/apply

    }    [HttpPost("apply")]

    public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponRequest request)

    // GET: api/coupon    {

    [HttpGet] var result = await _couponService.ApplyCouponAsync(request.Code, request.UserId, request.CartTotal, request.ItemCount);

    public async Task<ActionResult<List<Coupon>>> GetAllCoupons()        if (result != null)

    {        {

        try            return Ok(result);

        {        }

            var coupons = await _couponService.GetAllCouponsAsync(); return BadRequest(result);

return Ok(coupons);    }

        }}
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all coupons");
return StatusCode(500, new { message = "An error occurred while retrieving coupons" });
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
        _logger.LogError(ex, "Error getting coupon {CouponId}", id);
        return StatusCode(500, new { message = "An error occurred while retrieving the coupon" });
    }
}

// GET: api/coupon/code/{code}
[HttpGet("code/{code}")]
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
        _logger.LogError(ex, "Error getting coupon by code {CouponCode}", code);
        return StatusCode(500, new { message = "An error occurred while retrieving the coupon" });
    }
}

// POST: api/coupon
[HttpPost]
public async Task<ActionResult<Coupon>> CreateCoupon([FromBody] CouponCreateRequest request)
{
    try
    {
        var coupon = await _couponService.CreateCouponAsync(request);
        return CreatedAtAction(nameof(GetCouponById), new { id = coupon.Id }, coupon);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating coupon");
        return BadRequest(new { message = ex.Message });
    }
}

// PUT: api/coupon/{id}
[HttpPut("{id}")]
public async Task<ActionResult<Coupon>> UpdateCoupon(int id, [FromBody] CouponUpdateRequest request)
{
    try
    {
        var coupon = await _couponService.UpdateCouponAsync(id, request);
        return Ok(coupon);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating coupon {CouponId}", id);
        return BadRequest(new { message = ex.Message });
    }
}

// DELETE: api/coupon/{id}
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteCoupon(int id)
{
    try
    {
        await _couponService.DeleteCouponAsync(id);
        return NoContent();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error deleting coupon {CouponId}", id);
        return StatusCode(500, new { message = "An error occurred while deleting the coupon" });
    }
}
}
