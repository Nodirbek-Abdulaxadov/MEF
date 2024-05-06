using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.Entites;
using GEH.Exceptions;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController(AppDbContext dbContext)
    : ControllerBase
{
    private readonly TestService _testService = new(dbContext);

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var test = _testService.Get(id);
        return Ok(test);
    }

    [HttpPost]
    public IActionResult Post(Test test)
    {
        _testService.Add(test);
        return Ok();
    }
}
