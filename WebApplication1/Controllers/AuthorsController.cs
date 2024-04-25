using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.Entites;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController (AppDbContext dbContext)
    : ControllerBase
{
    [HttpGet]
    public IActionResult GetAuthors()
    {
        return Ok(dbContext.Authors);
    }

    [HttpGet("with-tests")]
    public IActionResult GetAuthorsWithTests()
    {
        return Ok(dbContext.Authors.Include(i => i.Tests));
    }

    [HttpGet("{id}")]
    public IActionResult GetAuthor(string id)
    {
        var author = dbContext.Authors.FirstOrDefault(i => i.Id == id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpPost]
    public IActionResult CreateAuthor(Author author)
    {
        dbContext.Authors.Add(author);
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAuthor(string id, Author author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }
        dbContext.Authors.Update(author);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAuthor(string id)
    {
        var author = dbContext.Authors.FirstOrDefault(i => i.Id == id);
        if (author == null)
        {
            return NotFound();
        }
        dbContext.Authors.Delete(id);
        return NoContent();
    }
}