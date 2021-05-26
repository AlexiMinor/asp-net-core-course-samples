using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;

namespace NewsAggregator.WebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
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

       
        [HttpGet]
        public async Task<IActionResult> Get(string name, string url, Guid sourseId)
        {
            var news = await _newsService.GetNewsBySourseId(null);

            return Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {

            return Ok();
        }
    }
}
