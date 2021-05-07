using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Models;
using NewsAggregator.Models.ViewModels.News;
using NewsAggregators.Services.Implementation;
using Serilog;

namespace NewsAggregator.Controllers
{
    //[Authorize(Policy = "18+Content")]
    //[Authorize(Roles = "user")]
    //[Authorize(Roles = "Admin, User")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICommentService _commentService;
        private readonly IRssSourseService _rssSourseService;
        private readonly OnlinerParser _onlinerParser;
        private readonly TutByParser _tutByParser;

        public NewsController(INewsService newsService, 
            IRssSourseService rssSourse,
            OnlinerParser onlinerParser, TutByParser tutByParser, 
            ICommentService commentService)
        {
            _newsService = newsService;
            _rssSourseService = rssSourse;
            _onlinerParser = onlinerParser;
            _tutByParser = tutByParser;
            _commentService = commentService;
        }

        // GET: News
        public async Task<IActionResult> Index(Guid? sourseId, int page = 1)
        {
            var news = (await _newsService.GetNewsBySourseId(sourseId))
                .ToList(); 
            //
            //TODO get information about all rssSourses from news 
            //Variant 1: add object to dto + get all news with rss sourse as inner rssSourse object (for each entity)
            //var rssSourses = news.Select(dto => dto.RssSourse).Distinct().ToList();
            // (size of dto)*News.Count 

            //Variant2
            var rssSourses = _rssSourseService.GetRssSoursesByIds(
                news.Select(dto => dto.RssSourseId.GetValueOrDefault()).Distinct().ToArray()); //(size of dto)*N(unique rss from news)


            var pageSize = 500;

            var newsPerPages = news.Skip((page - 1) * pageSize).Take(pageSize);

            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = news.Count
            };

            return View(new NewsListWithPaginationInfo()
            {
                IsAdmin = false,//_RoleService.IsUserIsAdmin(ClaimsPrincliple HttpContext.User)
                News = newsPerPages,
                PageInfo = pageInfo
            });
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourse = await _newsService.GetNewsWithRssSourseNameById(id.Value);

            if (sourse == null)
            {
                return NotFound();
            }

            var comments = await _commentService.GetCommentsByNewsId(sourse.Id);
            var viewModel = new NewsWithCommentsViewModel()
            {
                Id = sourse.Id,
                Article = sourse.Article,
                Body = sourse.Body,
                Summary = sourse.Summary,
                Url = sourse.Url,
                RssSourseId = sourse.RssSourseId,
                RssSourseName = sourse.RssSourseName,
                Comments = comments
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Aggregate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aggregate(CreateNewsViewModel sourse)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var rssSourses = await _rssSourseService
                    .GetAllRssSources();
                var newInfos = new List<NewsDto>(); // without any duplicate
                
                foreach (var rssSourse in rssSourses)
                {
                    var newsList = await _newsService
                        .GetNewsInfoFromRssSourse(rssSourse);

                    if (rssSourse.Id.Equals(new Guid("4B92ABBF-CAB0-493B-8320-857BD2901735")))
                    {
                        foreach (var newsDto in newsList)
                        {
                           var newsBody = await _onlinerParser.Parse(newsDto.Url);
                           newsDto.Body = newsBody;
                        }
                    }
                    else if (rssSourse.Id.Equals(new Guid("4B92ABBF-CAB0-493B-8320-857BD2901735")))
                    {
                        foreach (var newsDto in newsList)
                        {
                            var newsBody = await _onlinerParser.Parse(newsDto.Url);
                            newsDto.Body = newsBody;
                        }
                    }
                    newInfos.AddRange(newsList);
                }
                await _newsService.AddRange(newInfos);
                stopwatch.Stop();
                Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}");
            }

            catch (Exception e)
            {
                Log.Error(e, $"{e.Message}");

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: News/Create
        public async Task<IActionResult> Create()
        {
            
            var model = new CreateNewsViewModel()
            {
                Sources = new SelectList(await _rssSourseService.GetAllRssSources(), 
                    "Id", //field of element with value
                    "Name") //field of element with text
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNewsViewModel sourse)
        {
            if (ModelState.IsValid)
            {
                sourse.Id = Guid.NewGuid();
               
                //await _newsService.AddNews(sourse.);
                return RedirectToAction(nameof(Index));
            }
            return View(sourse);
        }
    }
}
