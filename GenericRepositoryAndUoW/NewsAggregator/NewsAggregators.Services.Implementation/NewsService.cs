using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregators.Services.Implementation
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<NewsDto>> FindNews()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsDto>> GetNewsBySourseId(Guid? id)
        {
            var news = id.HasValue 
                ? await _unitOfWork.News.FindBy(n 
                        => n.RssSourseId.Equals(id.GetValueOrDefault()))
                    .ToListAsync()
                : await _unitOfWork.News.FindBy(n => n.Id!=null).ToListAsync();


            return news.Select(n => new NewsDto()
            {
                Id = n.Id,
                Article = n.Article,
                Body = n.Body,
                RssSourseId = n.RssSourseId,
                Url = n.Url,
                Body2 = n.Body2
            }).ToList();
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            var entity = await _unitOfWork.News.GetById(id);
            return new NewsDto()
            {
                Id = entity.Id,
                Article = entity.Article,
                Body = entity.Body,
                RssSourseId = entity.RssSourseId,
                Url = entity.Url,
                Body2 = entity.Body2
            };
        }

        public async Task<NewsWithRssNameDto> GetNewsWithRssSourseNameById(Guid id)
        {
            var result = await _unitOfWork.News
                .FindBy(n => n.RssSourseId.HasValue, 
                    n=>n.RssSourse, n=>n.Comments)
                .Select(n => new NewsWithRssNameDto()
                {
                    Id = n.Id
                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task Test()
        {
            await _unitOfWork.News.AddRange(new List<News>());
            await _unitOfWork.RssSources.AddRange(new List<RssSourse>());
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task AddNews(NewsDto news)
        {
            var entity = new News()
            {
                Id = news.Id,
                Article = news.Article,
                Body = news.Body,
                RssSourseId = news.RssSourseId,
                Url = news.Url,
                Body2 = news.Body2
            };

            await _unitOfWork.News.AddRange(new []{entity});
        }

        public async Task AddRange(IEnumerable<NewsDto> news)
        {
            var entities = news.Select(ent => new News()
            {
                Id = ent.Id,
                Article = ent.Article,
                Body = ent.Body,
                RssSourseId = ent.RssSourseId,
                Url = ent.Url,
                Body2 = ent.Body2
            }).ToList();
            await _unitOfWork.News.AddRange(entities);
        }

        public async Task<int> EditNews(NewsDto news)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(NewsDto news)
        {
            throw new NotImplementedException();
        }

        private News ConvertToNewsFromDto(NewsDto dto)
        {
            return new News()
            {
                Id = dto.Id,
                Article = dto.Article,
                Body = dto.Body,
                RssSourseId = dto.RssSourseId,
                Url = dto.Url,
                Body2 = dto.Body2
            };
        }
    }
}
