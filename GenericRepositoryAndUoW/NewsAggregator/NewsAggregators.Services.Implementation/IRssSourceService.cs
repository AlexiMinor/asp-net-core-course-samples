using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Repositories.Implementation;

namespace NewsAggregator.Core.Services.Interfaces
{
    public class RssSourseService : IRssSourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RssSourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<RssSourseDto>> GetAllRssSources()
        {
            return await _unitOfWork.RssSources.FindBy(sourse => !string.IsNullOrEmpty(sourse.Name))
                .Select(sourse => new RssSourseDto()
                {
                    Id = sourse.Id,
                    Name = sourse.Name,
                    Url = sourse.Url
                }).ToListAsync();
        }

        public async Task<IEnumerable<RssSourseDto>> GetRssSoursesByIds(IEnumerable<Guid> ids)
        {
            return await _unitOfWork.RssSources.FindBy(sourse => ids.Contains(sourse.Id))
                .Select(sourse => _mapper.Map<RssSourseDto>(sourse)).ToListAsync();
        }

        public async Task<RssSourseDto> GetRssSourseById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddRssSourse(RssSourseDto news)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RssSourseDto>> AddRange(IEnumerable<RssSourseDto> news)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditRssSourse(RssSourseDto news)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(RssSourseDto news)
        {
            throw new NotImplementedException();
        }
    }
}
