using System.Collections.Generic;
using AutoMapper;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;

namespace NewsAggregators.Services.Implementation.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<News, NewsDto>();
            CreateMap<News, NewsWithRssNameDto>();

            //PLS DON'T DO SO
            //CreateMap<News, NewsWithRssNameDto>()
            //    .ForMember(dest =>
            //            dest.RssSourseName,
            //        opt => opt.MapFrom(src =>
            //            (_unitOfWork.RssSources.GetById(src.RssSourseId.GetValueOrDefault())).Result.Name));

            CreateMap<News, NewsWithRssNameDto>()
                .ForMember(dest =>
                        dest.RssSourseName,
                    opt => opt.MapFrom(src =>
                        (_unitOfWork.RssSources.GetById(src.RssSourseId.GetValueOrDefault())).Result.Name));

            CreateMap<News, CustomNewsDto>()
                .ForMember(dest=>
                    dest.Title, 
                    opt=>
                        opt.MapFrom(src=>src.Article))
                .ForMember(dest=>dest.CustomSpecificValue,
                    opt => opt
                        .MapFrom(src => src.Article.Replace("A", "B")));
                

            CreateMap<NewsDto, News>();
        }
    }
}