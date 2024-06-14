namespace EntityFrameworkCore.MongoDb;

public class Collection<T> where T : BaseEntity
{
    private readonly IMongoCollection<T>? _collection;

    public Collection(MongoDbContext dbContext)
    {
        _collection = dbContext.GetCollection<T>(typeof(T).Name);
    }

    public IFindFluent<T, T> AsFindFluent()
        => _collection.Find(_ => true);

    public List<T> ToList()
        => _collection.Find(FilterDefinition<T>.Empty).ToList();

    public Task<List<T>> ToListAsync()
        => _collection.Find(FilterDefinition<T>.Empty).ToListAsync();

    public T FirstOrDefault()
        => _collection.Find(FilterDefinition<T>.Empty).FirstOrDefault();

    public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        => _collection.Find(predicate).FirstOrDefault();

    public Task<T> FirstOrDefaultAsync()
        => _collection.Find(FilterDefinition<T>.Empty).FirstOrDefaultAsync();

    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        => _collection.Find(predicate).FirstOrDefaultAsync();

    public IFindFluent<T, T> Where(Expression<Func<T, bool>> predicate)
        => _collection.Find(predicate);

    public bool Any()
        => _collection.Find(FilterDefinition<T>.Empty).Any();

    public bool Any(Expression<Func<T, bool>> predicate)
        => _collection.Find(predicate).Any();

    public long Count()
        => _collection!.CountDocuments(FilterDefinition<T>.Empty);

    public void Add(T entity)
        => _collection!.InsertOne(entity);

    public Task AddAsync(T entity)
        => _collection!.InsertOneAsync(entity);

    public void AddRange(IEnumerable<T> entities)
        => _collection!.InsertMany(entities);

    public Task AddRangeAsync(IEnumerable<T> entities)
        => _collection!.InsertManyAsync(entities);

    public void Update(T entity)
        => _collection!.ReplaceOne(Builders<T>.Filter.Eq("_id", entity.Id), entity);

    public Task UpdateAsync(T entity)
        => _collection!.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", entity.Id), entity);

    public void Delete(string id)
        => _collection!.DeleteOne(Builders<T>.Filter.Eq("_id", id));

    public Task DeleteAsync(string id)
        => _collection!.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));

    public List<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
    {
        var baseCollection = ToList();
        //Soon
        /*var propertyCollection = baseCollection.Select(navigationPropertyPath.Compile()).ToList();

        foreach (var item in baseCollection)
        {
            var property = propertyCollection.FirstOrDefault();
            if (property != null)
            {
                var propertyType = property.GetType();
                var propertyValue = propertyType.GetProperty("Id")!.GetValue(property);
                var propertyItem = baseCollection.FirstOrDefault(x => x.Id == propertyValue!.ToString());
                propertyType.GetProperty(typeof(T).Name)!.SetValue(property, propertyItem);
            }
        }*/
        return baseCollection;
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
}

public static class CollectionExtensions
{
    public static IFindFluent<T, T> OrderBy<T>(this IFindFluent<T, T> findFluent, 
                                             Expression<Func<T, object>> expression)
        => findFluent.SortBy(expression);


    public static IFindFluent<T, T> OrderByDescending<T>(this IFindFluent<T, T> findFluent,
                                                       Expression<Func<T, object>> expression)
        => findFluent.SortByDescending(expression);

    public static List<T> Take<T>(this IFindFluent<T, T> findFluent, int count)
    {
        return findFluent.Limit(count).ToList();
    }
}