using ECommerceAfaq.Application.DTOs.Order;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUser;

        public OrderController(IOrderService orderService, ICurrentUserService currentUser)
        {
            _orderService = orderService;
            _currentUser = currentUser;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<OrderResponseDto>> Checkout(CheckoutDto dto)
        {
            try
            {

                var order = await _orderService.CheckOutAsync(_currentUser.UserId!, dto); 
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> GetMyOrders()
        {
            var orders = await _orderService.GetUserOrdersAsync(_currentUser.UserId!); 
            return Ok(orders);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDto>> GetById(int id)
        {
            var isAdmin = User.IsInRole("Admin");
            var order = await _orderService.GetByIdAsync(_currentUser.UserId!, id, isAdmin); 
            return order is null ? NotFound() : Ok(order);
        }


        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var cancelled = await _orderService.CancelOrderAsync(_currentUser.UserId!, id);
                return cancelled ? NoContent() : NotFound(); 
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatus newStatus)
        {
            var updated = await _orderService.UpdateStatusAsync(id, newStatus);
            return updated ? NoContent() : NotFound();
        }
    }
}
