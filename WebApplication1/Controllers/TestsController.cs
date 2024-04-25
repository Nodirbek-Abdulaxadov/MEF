using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.Entites;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController (AppDbContext dbContext)
    : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var tests = dbContext.Tests.ToList();
        return Ok(tests);
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var test = dbContext.Tests.FirstOrDefault(t => t.Id == id);
        if (test is null)
            return NotFound();
        return Ok(test);
    }

    [HttpPost]
    public IActionResult Post(Test test)
    {
        dbContext.Tests.Add(test);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put(string id, Test test)
    {
        var existingTest = dbContext.Tests.FirstOrDefault(t => t.Id == id);
        if (existingTest is null)
            return NotFound();
        test.Id = id;
        dbContext.Tests.Update(test);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingTest = dbContext.Tests.FirstOrDefault(t => t.Id == id);
        if (existingTest is null)
            return NotFound();
        dbContext.Tests.Delete(id);
        return Ok();
    }
}
