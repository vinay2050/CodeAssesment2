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
            await _newsService.RefereshChache();
            var data = await _newsService.GetNews(title ?? "", pageNumber ?? 1, pageSize ?? 10);
            return Ok(data);
        }
    }
}
