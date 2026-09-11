using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // POST: api/Posters/upload
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


    // GET: api/Posters
    [HttpGet]
    public async Task<IActionResult> GetPosters()
    {
        var posters = await _context.Posters
            .Select(p => new
            {
                p.Id,
                p.FileName,
                p.ContentType
            })
            .ToListAsync();

        return Ok(posters);
    }


    // GET: api/Posters/1/image
    [HttpGet("{id}/image")]
    public async Task<IActionResult> GetPosterImage(int id)
    {
        var poster = await _context.Posters
            .FirstOrDefaultAsync(p => p.Id == id);

        if (poster == null)
        {
            return NotFound();
        }

        return File(
            poster.Data,
            poster.ContentType
        );
    }
}