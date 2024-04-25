using MongoDB.Querying;
using WebApplication1.Data.Entites;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public Collection<Test> Tests { get; set; }
    public Collection<Author> Authors { get; set; }
}