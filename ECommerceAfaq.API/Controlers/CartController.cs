using ECommerceAfaq.Application.DTOs.Cart;
using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUser;

        public CartController(ICartService cartService , ICurrentUserService currentUser)
        {
           _cartService = cartService;
           _currentUser = currentUser;
        }
        [HttpGet]
        public async Task<ActionResult<CartItemResponseDto>> GetCart()
       {
            var cart = await _cartService.GetCartAsync(_currentUser.UserId!);
            return Ok(cart);
        }
        [HttpPost("items")]
        public async Task<ActionResult<CartItemResponseDto>> AddItem(AddCartItemDto dto)
        {
            try
            {
                var cart = await _cartService.AddItemAsync(_currentUser.UserId! , dto);
                return Ok(cart);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("items/{productId:int}")]
        public async Task<IActionResult> UpadteItem(int productId ,UpdateCartItemDto dto)
        {
            try
            {
                var updated = await _cartService.UpdateItemsAsync(_currentUser.UserId!,productId, dto);
                return updated ? NoContent() : NotFound();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                var removed = await _cartService.RemoveItemAsync(_currentUser.UserId!, productId);
                return removed ? NoContent() : NotFound();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
        }


        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
        
            
                var cleared = await _cartService.ClearCartAsync(_currentUser.UserId!);
                return cleared ? NoContent() : NotFound();

        }
    }
}
