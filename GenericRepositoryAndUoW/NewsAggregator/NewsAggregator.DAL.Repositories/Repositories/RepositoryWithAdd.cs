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
    public abstract class RepositoryWithAdd<T> : Repository<T>, IRepositoryWithAdd<T> where T : class, IBaseEntity
    {
        protected RepositoryWithAdd(NewsAggregatorContext context) : base(context)
        {
        }

        public void Add(T t)
        {
            Table.Add(t);
        }
    }
}