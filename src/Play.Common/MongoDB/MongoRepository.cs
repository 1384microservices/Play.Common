using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Play.Common.MongoDB;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> collection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        collection = database.GetCollection<T>(collectionName);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {

        var filter = filterBuilder.Empty;
        var items = await collection.Find(filter).ToListAsync();
        return items;
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        var items = await collection.Find(filter).ToListAsync();
        return items;
    }

    public async Task<T> GetOneAsync(Guid id)
    {
        var filter = filterBuilder.Eq(entity => entity.Id, id);
        var item = await collection.Find(filter).SingleOrDefaultAsync();
        return item;
    }


    public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter)
    {
        var item = await collection.Find(filter).SingleOrDefaultAsync();
        return item;
    }

    public async Task CreateAsync(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        await collection.InsertOneAsync(item);
    }

    public async Task UpdateAsync(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var filter = filterBuilder.Eq(entity => entity.Id, item.Id);
        await collection.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteAsync(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var filter = filterBuilder.Eq(entity => entity.Id, item.Id);
        await collection.DeleteOneAsync(filter);
    }



}