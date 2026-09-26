using ECommerceAfaq.Application.DTOs.Address;
using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ICurrentUserService _currentUser;

        public AddressesController(IAddressService addressService , ICurrentUserService currentUser)
        {
         _addressService = addressService;
         _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<ActionResult<AddressResponseDto>> GetAll()
        {
            var addresses = await _addressService.GetUserAddressesAsync(_currentUser.UserId!);
            return Ok(addresses);
        }

        [HttpPost]

        public async Task<ActionResult<AddressResponseDto>> Add(AddressCreateDto dto)
        {
            var created = await _addressService.AddAsync(_currentUser.UserId!, dto); 
            return Ok(created);
        }
        [HttpPut("{id:int}")]

        public async Task<IActionResult> Update( int id, AddressCreateDto dto)
        {
            var updated = await _addressService.UpdateAsync(_currentUser.UserId!, id, dto);
            return updated ? NoContent() : NotFound(); 


        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _addressService.DeleteAsync(_currentUser.UserId!, id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("{id:int}/set-default")]

        public async Task<IActionResult> SetDefault(int id)
        {
            var updated = await _addressService.SetDefaultAsync(_currentUser.UserId!, id);
            return updated ? NoContent() : NotFound();
        }


    }
}
