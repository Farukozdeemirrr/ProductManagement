
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Abstract;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private const string GetCategoryByIdRouteName = "GetCategoryById";

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            var response = await _categoryService.CreateAsync(createDto, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return CreatedAtRoute(GetCategoryByIdRouteName, new { id = response.Data!.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var response = await _categoryService.UpdateAsync(id, updateDto, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id:guid}", Name = GetCategoryByIdRouteName)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetAllAsync(cancellationToken);

            return Ok(response);
        }

        [HttpPatch("{id:guid}/toggle-active")]
        public async Task<IActionResult> ToggleActiveStatusAsync([FromRoute] Guid id, [FromQuery] string? updatedBy, CancellationToken cancellationToken)
        {
            var response = await _categoryService.ToggleActiveStatusAsync(id, updatedBy, cancellationToken);

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }
    }
}
