using BlogApp.Core.Domain.Interfaces;
using BlogApp.Core.Domain.Models;
using BlogApp.Core.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogEntriesController : ControllerBase
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IBlobStorageService _blobStorageService;

        public BlogEntriesController(IBlogEntryRepository blogEntryRepository, IBlobStorageService blobStorageService)
        {
            _blogEntryRepository = blogEntryRepository;
            _blobStorageService = blobStorageService;
        }

        // Obtener todas las entradas de blog
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _blogEntryRepository.GetAll();
            return Ok(entries);
        }

        // Obtener una entrada de blog por ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _blogEntryRepository.GetById(id);
            if (entry == null)
            {
                return NotFound(new { message = "Blog entry not found" });
            }

            return Ok(entry);
        }

        // Crear una nueva entrada de blog
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BlogEntry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            string rawUserId = HttpContext.User.FindFirstValue("id");
            if (!int.TryParse(rawUserId, out int userId))
            {
                return Unauthorized();
            }
            entry.idUser = userId;
            await _blogEntryRepository.Create(entry);
            return CreatedAtAction(nameof(GetById), new { id = entry.IdEntry }, entry);
        }

        // Actualizar una entrada de blog
        [Authorize]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogEntry entry)
        {
            if (id != entry.IdEntry)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var existingEntry = await _blogEntryRepository.GetById(id);
            if (existingEntry == null)
            {
                return NotFound(new { message = "Blog entry not found" });
            }

            await _blogEntryRepository.Update(entry);
            return Ok(new { message = "Entry updated successfully" });
        }

        // Eliminar una entrada de blog por ID
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _blogEntryRepository.Delete(id);
            if (result == 0)
            {
                return NotFound(new { message = "Blog entry not found" });
            }

            return NoContent();
        }
              [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No image provided.");

            var fileName = Path.GetFileName(imageFile.FileName);
            using var stream = imageFile.OpenReadStream();
            var imageUrl = await _blobStorageService.UploadImageAsync(stream,fileName);

            return Ok(new { imageUrl = imageUrl });
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new { errors });
        }
    }

}
