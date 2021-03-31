using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class, IBaseEntity
    {
        Task<T> GetById(Guid id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes); 
        IQueryable<T> Get(); 

        Task Add(T news);
        Task AddRange(IEnumerable<T> entities);

        Task Update(T news);
        Task Remove(Guid id);
        Task RemoveRange(IEnumerable<T> news);
    }
}