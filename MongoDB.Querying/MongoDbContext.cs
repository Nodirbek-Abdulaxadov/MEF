namespace EntityFrameworkCore.MongoDb;

public abstract class MongoDbContext
{
    private readonly MongoDbContextOptions _options;
    private IMongoClient _client;
    private IMongoDatabase _database;

    public MongoDbContext() : this(new MongoDbContextOptions()) { }

    public MongoDbContext(MongoDbContextOptions options)
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