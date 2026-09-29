using ECommerceAfaq.Application.DTOs.WishList;
using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly ICurrentUserService _currentUserService;

        public WishListController( IWishListService wishListService , ICurrentUserService currentUserService)
        {
            _wishListService = wishListService;
            _currentUserService = currentUserService;
        }

        [HttpGet] 
        public async Task<ActionResult<List<WishListItemResponseDto>>> GetAll()
        {
            var items = await _wishListService.GetWishListAsync(_currentUserService.UserId!); 
            return Ok(items);
        }


        [HttpPost("{productId:int}")]
        public async Task<ActionResult<WishListItemResponseDto>> Add(int productId)
        {

            try
            {
                var item = await _wishListService.AddAsync(_currentUserService.UserId!, productId);
                return Ok(item);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Remove(int productId)
        {

  
                var removed = await _wishListService.RemoveAsync(_currentUserService.UserId!, productId);
                return removed ? NoContent() : NotFound();

        }


        [HttpGet("{productId:int}/exists")]
        public async Task<IActionResult> Exists(int productId)
        {


            var exists = await _wishListService.IsInWishListAsync(_currentUserService.UserId!, productId);
            return Ok(exists);

        }



    }
}
