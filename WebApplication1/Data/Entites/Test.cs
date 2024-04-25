using MongoDB.Querying;

namespace WebApplication1.Data.Entites;

public class Test : BaseModel
{
    public string Name { get; set; } = default!;
    public int Number { get; set; }

    public string AuthorId { get; set; } = default!;
}