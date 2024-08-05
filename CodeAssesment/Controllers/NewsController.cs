using CodeAssesment.Model;
using CodeAssesment.Service.Interface;
using CodeAssesment.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeAssesment.API.Controllers
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


        [HttpGet]
        [Route("GetStories")]
        public async Task<IActionResult> GetStories([FromQuery] string? title, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
           var data= await _newsService.GetNews(title, pageNumber??1, pageSize??10);
            return Ok(data);
        }


        [HttpPost]
        [Route("RefreshCache")]
        public async Task<IActionResult> RefreshCache()
        {
            await _newsService.RefereshChache();
            return Ok("Success");
        }

    }
}
