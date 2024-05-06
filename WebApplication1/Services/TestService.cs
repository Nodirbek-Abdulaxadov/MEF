using GEH.Exceptions;
using WebApplication1.Data;
using WebApplication1.Data.Entites;

namespace WebApplication1.Services;

public class TestService(AppDbContext dbContext)
{
    public void Add(Test test)
    {
        if (test is null)
            throw new FailedException("Test is null");

        if (test.Name is null || test.Name.Length < 3 || test.Number < 0)
            throw new FailedException("Test is invalid");

        dbContext.Tests.Add(test);
    }

    public Test Get(string id)
    {
        var test = dbContext.Tests.FirstOrDefault(t => t.Id == id);
        if (test is null)
            throw new NotFoundException();
        return test;
    }
}
