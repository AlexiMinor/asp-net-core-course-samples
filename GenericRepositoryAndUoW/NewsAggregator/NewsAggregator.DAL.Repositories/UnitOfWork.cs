using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsAggregatorContext _db;
        private readonly IRepository<News> _newsRepository; 
        private readonly IRepository<User> _usersRepository; 
        private readonly IRepository<Role> _rolesRepository; 
        private readonly IRepository<RssSourse> _rssRepository;
        private readonly IRepository<Comment> _commentRepository;


        public UnitOfWork(NewsAggregatorContext db,
            IRepository<News> newsRepository, 
            IRepository<RssSourse> rssRepository, 
            IRepository<User> usersRepository, 
            IRepository<Role> rolesRepository, 
            IRepository<Comment> commentRepository)
        {
            _db = db;
            _newsRepository = newsRepository;
            _rssRepository = rssRepository;
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _commentRepository = commentRepository;
        }

        public IRepository<News> News => _newsRepository;
        public IRepository<Comment> Comments => _commentRepository;
        public IRepository<RssSourse> RssSources => _rssRepository;
        public IRepository<User> Users => _usersRepository;
        public IRepository<Role> Roles => _rolesRepository;

       

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
