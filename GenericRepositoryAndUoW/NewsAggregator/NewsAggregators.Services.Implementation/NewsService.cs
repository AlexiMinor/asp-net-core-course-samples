using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public NewsService(IUnitOfWork unitOfWork,
            IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsDto>> GetNewsBySourseId(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    Log.Warning("Id in NewsService.GetNewsBySourseId was null");
                }

                var news = id.HasValue
                    ? await _unitOfWork.News.FindBy(
                            n => n.RssSourseId.Equals(id.GetValueOrDefault()))
                        .ToListAsync()
                    : await _unitOfWork.News.FindBy(n => n != null)
                        .ToListAsync();

                return news.Select(n => _mapper.Map<NewsDto>(n)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            var entity = await _unitOfWork.News.GetById(id);
            return _mapper.Map<NewsDto>(entity);
        }

        public async Task<NewsWithRssNameDto> GetNewsWithRssSourseNameById(Guid id)
        {
            var result = await _unitOfWork.News
                .FindBy(n => n.Id.Equals(id),
                    n => n.RssSourse).FirstOrDefaultAsync();

            var mappingResult = _mapper.Map<NewsWithRssNameDto>(result);

            return mappingResult;
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
                        if (!currentNewsUrls.Any(url => url.Equals(syndicationItem.Id)))
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

            await _unitOfWork.News.AddRange(new[] { entity });
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
