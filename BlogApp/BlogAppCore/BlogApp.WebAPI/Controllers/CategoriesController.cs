using BlogApp.Core.Domain.Interfaces;
using BlogApp.Core.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Obtener todas las categorías
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAll();
            return Ok(categories);
        }

        // Obtener una categoría por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            return Ok(category);
        }

        // Crear una nueva categoría
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            await _categoryRepository.Create(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // Actualizar una categoría existente
        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var existingCategory = await _categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            await _categoryRepository.Update(category);
            return Ok(new { message = "Category updated successfully" });
        }

        // Eliminar una categoría por ID
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryRepository.Delete(id);
            if (result == 0)
            {
                return NotFound(new { message = "Category not found" });
            }

            return NoContent();
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new { errors });
        }
    }

}
