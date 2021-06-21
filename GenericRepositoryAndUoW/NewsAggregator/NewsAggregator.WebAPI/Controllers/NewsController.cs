using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;

namespace NewsAggregator.WebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(400, Type = typeof(BadRequestResponse))]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

            var news = await _newsService.GetNewsById(id);

            //if (news == null)
            //{
            //    return NotFound();
            //}
            return Ok(news);
        }

        
        /// <summary>
        /// Get news from database
        /// </summary>
        /// <returns>News from DB</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsDto>))]
        public async Task<IActionResult> Get()
        {
            var user = HttpContext.User;
            var news = await _newsService.GetTopRatedNews();

            return Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {

            return Ok();
        }
    }
}
