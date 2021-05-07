using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.Core.Services.Interfaces;

namespace NewsAggregator.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> List(Guid newsId)
        {
            var comments = await _commentService.GetCommentsByNewsId(newsId);

            return View(comments);
        }
    }
}
