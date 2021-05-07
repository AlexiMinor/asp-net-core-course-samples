using System;
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
            CreateMap<News, NewsWithRssNameDto>()
                .ForMember(dto => dto.RssSourseName,
                    opt
                        =>opt.MapFrom(news => news.RssSourse.Name)
                );
            CreateMap<RssSourse, RssSourseDto>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();

        }
    }
}