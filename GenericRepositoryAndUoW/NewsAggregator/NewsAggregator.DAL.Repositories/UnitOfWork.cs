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
        private readonly IRepository<RssSourse> _rssRepository;


        public UnitOfWork(NewsAggregatorContext db,
            IRepository<News> newsRepository, 
            IRepository<RssSourse> rssRepository)
        {
            _db = db;
            _newsRepository = newsRepository;
            _rssRepository = rssRepository;
        }

        public IRepository<News> News => _newsRepository;
        public IRepository<RssSourse> RssSources => _rssRepository;

       

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
