using ECommerceAfaq.Application.DTOs.Product;
using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
         public async Task<ActionResult<List<ProductResponseDto>>> GetAll([FromQuery] ProductQueryParameters parameters)
        {
            var products = await _productServices.GetFilterdAsync(parameters);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _productServices.GetByIdAsync(id);

            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create(ProductCreateDto dto)
        {
            try
            {
                var created = await _productServices.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id , ProductUpdateDto dto)
        {
            try
            {

                var updated  = await _productServices.UpdateAsync(id, dto);
                return updated ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete (int id)
        {
            var deleted = await _productServices.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("{id:int}/image")]
        public async Task<IActionResult> UploadImage (int id , IFormFile file)
        {
            try
            {
                var uploaded = await _productServices.UploadImageAsync(id, file);
                return uploaded ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
