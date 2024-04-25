namespace MongoDB.Querying;

public class DbContext
{
    private readonly DbContextOptions _options;
    private IMongoClient _client;
    private IMongoDatabase _database;

    public DbContext() : this(new DbContextOptions()) { }

    public DbContext(DbContextOptions options)
    {
        _options = options;
        _client = new MongoClient(_options.ConnectionString);
        _database = _client.GetDatabase(_options.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}