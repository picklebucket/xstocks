using MongoDB.Bson;
using MongoDB.Driver;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        protected StocksContext Context { get; }

        protected abstract string CollectionName { get; }

        protected BaseRepository(StocksContext context)
        {
            Context = context;
        }
        
        public virtual async Task CreateAsync(T entity)
        {
            IMongoCollection<T> collection = Context.GetCollection<T>(CollectionName);
            await collection.InsertOneAsync(entity).ConfigureAwait(false);
        }

        public async Task CreateManyAsync(params T[] entities)
        {
            IMongoCollection<T> collection = Context.GetCollection<T>(CollectionName);
            await collection.InsertManyAsync(entities).ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            IMongoCollection<T> collection = Context.GetCollection<T>(CollectionName);
            return await collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            IMongoCollection<T> collection = Context.GetCollection<T>(CollectionName);
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            await collection.DeleteOneAsync(filter).ConfigureAwait(false);
        }
    }
}
