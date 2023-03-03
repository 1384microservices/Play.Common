using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Play.Common;

public interface IRepository<T> where T : IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<T> GetOneAsync(Guid id);
    Task<T> GetOneAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T item);
    Task DeleteAsync(T item);
    Task UpdateAsync(T item);
}
