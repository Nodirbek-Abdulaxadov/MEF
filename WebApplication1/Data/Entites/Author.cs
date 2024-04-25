using MongoDB.Querying;

namespace WebApplication1.Data.Entites;

public class Author : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public List<Test> Tests { get; set; } = new();
}