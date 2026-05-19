using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Abstract;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private const string GetProductByIdRouteName = "GetProductById";

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductCreateDto createDto, CancellationToken cancellationToken)
        {
            var response = await _productService.CreateAsync(createDto, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return CreatedAtRoute(GetProductByIdRouteName, new { id = response.Data!.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] ProductUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var response = await _productService.UpdateAsync(id, updateDto, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id:guid}", Name = GetProductByIdRouteName)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var response = await _productService.GetByIdAsync(id, cancellationToken);

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var response = await _productService.GetAllAsync(cancellationToken);

            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterAsync([FromQuery] ProductFilterDto filterDto, CancellationToken cancellationToken)
        {
            var response = await _productService.FilterAsync(filterDto, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("{id:guid}/toggle-live")]
        public async Task<IActionResult> ToggleLiveStatusAsync([FromRoute] Guid id, [FromQuery] string? updatedBy, CancellationToken cancellationToken)
        {
            var response = await _productService.ToggleLiveStatusAsync(id, updatedBy, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("{id:guid}/toggle-active")]
        public async Task<IActionResult> ToggleActiveStatusAsync([FromRoute] Guid id, [FromQuery] string? updatedBy, CancellationToken cancellationToken)
        {
            var response = await _productService.ToggleActiveStatusAsync(id, updatedBy, cancellationToken);

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }
    }
}
