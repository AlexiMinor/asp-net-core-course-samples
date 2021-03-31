using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using Serilog;

namespace NewsAggregators.Services.Implementation
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public NewsService(IUnitOfWork unitOfWork, 
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
       
        public async Task<IEnumerable<NewsDto>> GetNewsBySourseId(Guid? id)
        {
            if (!id.HasValue)
            {
                Log.Warning("Id in NewsService.GetNewsBySourseId was null");
            }

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
                Summary = n.Summary
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
                Summary = entity.Summary
            };
        }

        public async Task<NewsWithRssNameDto> GetNewsWithRssSourseNameById(Guid id)
        {
            var result = await _unitOfWork.News
                .FindBy(n => n.RssSourseId.HasValue, 
                    n=>n.RssSourse, n=>n.Comments)
                .Select(n => new NewsWithRssNameDto()
                {
                    Id = n.Id,
                    Article = n.Article,
                    Body = n.Body,
                    Summary = n.Summary,
                    Url = n.Url,
                    RssSourseId = n.RssSourseId,
                    RssSourseName = n.RssSourse.Name
                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSourse(RssSourseDto rssSourse)
        {
            var news = new List<NewsDto>();
            using (var reader = XmlReader.Create(rssSourse.Url))
            {
                var feed = SyndicationFeed.Load(reader);
                reader.Close();
                if (feed.Items.Any())
                {
                    var currentNewsUrls = await _unitOfWork.News
                        .Get()//rssSourseId must be not nullable
                        .Select(n => n.Url)
                        .ToListAsync();

                    foreach (var syndicationItem in feed.Items)
                    {
                        if (!currentNewsUrls.Any(url=>url.Equals(syndicationItem.Id)))
                        {
                            var newsDto = new NewsDto()
                            {
                                Id = Guid.NewGuid(),
                                RssSourseId = rssSourse.Id,
                                Url = syndicationItem.Id,
                                Article = syndicationItem.Title.Text,
                                Summary = syndicationItem.Summary.Text //clean from html(?)
                            };
                            news.Add(newsDto);
                        }
                    }
                }
               
            }

            return news;

        }

        public async Task AddOnlinerNews(NewsDto news)
        {
            var entity = new News()
            {
                Id = news.Id,
                Article = news.Article,
                Body = news.Body,
                RssSourseId = news.RssSourseId,
                Url = news.Url,
                Summary = news.Summary
            };

            await _unitOfWork.News.AddRange(new[] { entity });
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
                Summary = news.Summary
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
                Summary = ent.Summary
            }).ToList();
            await _unitOfWork.News.AddRange(entities);
            await _unitOfWork.SaveChangesAsync();
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
                Summary = dto.Summary
            };
        }
    }
}
