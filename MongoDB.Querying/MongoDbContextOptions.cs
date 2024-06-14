namespace EntityFrameworkCore.MongoDb;

public class MongoDbContextOptions
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "test";

    public MongoDbContextOptions() { }

    public MongoDbContextOptions(string connectionString, string databaseName)
    {
        ConnectionString = connectionString;
        DatabaseName = databaseName;
    }
}