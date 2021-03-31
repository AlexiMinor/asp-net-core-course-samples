using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetNewsBySourseId(Guid? id);
        Task<NewsDto> GetNewsById(Guid id);
        Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSourse(RssSourseDto rssSourse);
        Task<NewsWithRssNameDto> GetNewsWithRssSourseNameById(Guid id);

        Task AddNews(NewsDto news);
        Task AddRange(IEnumerable<NewsDto> news);

        Task<int> EditNews(NewsDto news);
        Task<int> Delete(NewsDto news);

    }
}
