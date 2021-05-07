using System;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<News> News { get; }
        IRepository<Comment> Comments { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<RssSourse> RssSources { get; }

        Task<int> SaveChangesAsync();
    }
}