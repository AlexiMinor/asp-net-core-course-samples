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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid id)
        {
            return await _unitOfWork.Comments
                .FindBy(comment => comment.NewsId.Equals(id))
                .Select(comment => _mapper.Map<CommentDto>(comment)).ToListAsync();
        }

        public async Task AddComment(CommentDto comment)
        {
            await _unitOfWork.Comments.Add(_mapper.Map<Comment>(comment));
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
