using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly NewsAggregatorContext Db;
        protected readonly DbSet<T> Table;

        protected Repository(NewsAggregatorContext context)
        {
            Db = context;
            Table = Db.Set<T>(); //return table with type T ->GetSet(Type t)
        }
        public async Task<T> GetById(Guid id)
        {
            return await Table.FirstOrDefaultAsync(news => news.Id.Equals(id));
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includes)
        {
            var result = Table.Where(predicate);
            if (includes.Any())
            {
                result = includes
                    .Aggregate(result, 
                        (current, include) 
                            => current.Include(include));
            }

            return result;
        }

        public IQueryable<T> Get()
        {
            return Table;
        }

        public async Task Add(T news)
        {
            throw new NotImplementedException();
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            Table.AddRange(entities);

        }

        public async Task Update(T news)
        {
            throw new NotImplementedException();
        }

        public async Task Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRange(IEnumerable<T> news)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}