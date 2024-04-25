namespace MongoDB.Querying;

public static class DbContextExtensions
{
    public static void AddDbContext<TDbContext>(this IServiceCollection services, DbContextOptions options)
        where TDbContext : DbContext
    {
        services.AddSingleton(provider =>
        {
            var client = new MongoClient(options.ConnectionString);
            var database = client.GetDatabase(options.DatabaseName);

            // Create an instance of TDbContext using the provided options
            var dbContext = Activator.CreateInstance(typeof(TDbContext), options) as TDbContext;

            // Get all properties of TDbContext
            var properties = typeof(TDbContext).GetProperties();
            foreach (var property in properties)
            {
                // Check if the property type is a generic collection
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Collection<>))
                {
                    // Get the generic type argument of the collection
                    var entityType = property.PropertyType.GetGenericArguments()[0];

                    // Create an instance of Collection<TEntity> with the entity type and database instance
                    var collectionInstance = Activator.CreateInstance(typeof(Collection<>).MakeGenericType(entityType), dbContext);

                    // Set the collection instance to the property
                    property.SetValue(dbContext, collectionInstance);
                }
            }

            return dbContext;
        });

    }
}