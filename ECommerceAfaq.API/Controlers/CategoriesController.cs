using ECommerceAfaq.Application.DTOs.Category;
using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET api/categories
        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        // GET api/categories/10
        [HttpGet("{id:int}")]

        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category is null)
                return NotFound();

            return Ok(category);



        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryResponseDto>> Create(CategoryCreateDto dto)
        {
            var created = await _categoryService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var updated = await _categoryService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        //api/categories/1/image
        [HttpPost("{id:int}/image")]
        [Authorize(Roles = "Admin")]
        public  async Task<IActionResult> UploadImage(int id  , IFormFile  file)
        {
            try
            {
                var uploaded = await _categoryService.UploadImageAsync(id, file);

                if (!uploaded)
                    return NotFound();

                return NoContent();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }


        }



    }
}
