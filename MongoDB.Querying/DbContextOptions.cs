namespace MongoDB.Querying;

public class DbContextOptions
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "test";

    public DbContextOptions() { }

    public DbContextOptions(string connectionString, string databaseName)
    {
        ConnectionString = connectionString;
        DatabaseName = databaseName;
    }
}