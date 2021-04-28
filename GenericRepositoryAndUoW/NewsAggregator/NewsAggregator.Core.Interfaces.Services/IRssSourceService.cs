using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IRssSourseService
    {
        Task<IEnumerable<RssSourseDto>> GetAllRssSources();
        Task<RssSourseDto> GetRssSourseById(Guid id);
        Task<IEnumerable<RssSourseDto>> GetRssSoursesByIds(IEnumerable<Guid> ids);
        Task<int> AddRssSourse(RssSourseDto news);
        Task<IEnumerable<RssSourseDto>> AddRange(IEnumerable<RssSourseDto> news);

        Task<int> EditRssSourse(RssSourseDto news);
        Task<int> Delete(RssSourseDto news);

    }
}
