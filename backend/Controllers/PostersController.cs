using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostersController : ControllerBase
{
    private readonly AppDbContext _context;

    public PostersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Geen bestand ontvangen.");
        }

        using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);

        var poster = new Poster
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Data = memoryStream.ToArray()
        };

        _context.Posters.Add(poster);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Affiche opgeslagen.",
            id = poster.Id
        });
    }
}