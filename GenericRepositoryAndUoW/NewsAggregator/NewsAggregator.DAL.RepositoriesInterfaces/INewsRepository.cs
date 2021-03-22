using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Interfaces
{
    public interface IRepositoryWithAdd<T> : IRepository<T> where T : class, IBaseEntity
    {
        void Add(T entity);
    }

}
