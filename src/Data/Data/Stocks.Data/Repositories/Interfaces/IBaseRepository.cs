using MongoDB.Bson;
using Stocks.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T>
        where T: BaseEntity
    {
        Task CreateAsync(T entity);
        Task CreateManyAsync(params T[] entities);
        Task<T> GetByIdAsync(ObjectId id);
        Task DeleteAsync(ObjectId id);
    }
}
