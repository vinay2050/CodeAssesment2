using CodeAssesment.Model;
using CodeAssesment.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAssesment.Service.Services
{
    public class NewsService: INewsService
    {
        private readonly IHackerNewsService _service;
        private readonly ConfigeData _configeData;
        public NewsService(IHackerNewsService service, ConfigeData configeData)
        {
            _service = service;
            _configeData = configeData;
        }

        public async Task<GetStoriesResponse> GetNews(string title,int pageNumber, int pageSize)
        {
            GetStoriesResponse getStoriesResponse = new GetStoriesResponse();
            try
            {
                List<HackerStoryDetailsResponse> data = new List<HackerStoryDetailsResponse>();
                if (!string.IsNullOrEmpty(title))
                {
                    data = _configeData.NewsData.Where(s => s.title.Contains(title, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                else
                {
                    data = _configeData.NewsData;
                }
                var filterData=GetPage(data, pageNumber, pageSize);
                getStoriesResponse.TotalCount = data.Count;
                getStoriesResponse.Data = new();

                foreach( var  item in filterData) 
                {
                    getStoriesResponse.Data.Add(new GetStoriesDataResponse 
                    {
                         Title = item.title,
                          Url = item.url,
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine("An error occurred while Getting Stories Response: " + ex.Message); }
            return getStoriesResponse;
        }
       private IList<HackerStoryDetailsResponse> GetPage(IList<HackerStoryDetailsResponse> list, int page, int pageSize)
        {
            return list.Skip((page-1) * pageSize).Take(pageSize).ToList();
        }

        public async Task RefereshChache()
        {
            if (_configeData.NewsData == null)
            {
                _configeData.NewsData = new List<HackerStoryDetailsResponse>();
                try
                {
                    var newsIds = await _service.GetTopStories();
                    var tasks = newsIds.Select(id => GetStoryDetailsAsync(id)).ToArray();
                    var storyDetailsList = await Task.WhenAll(tasks);
                    _configeData.NewsData.AddRange(storyDetailsList.Where(details => details != null));
                }
                catch (Exception ex) { Console.WriteLine("An error occurred while refreshing the cache: " + ex.Message); }
            }
        }

        private async Task<HackerStoryDetailsResponse> GetStoryDetailsAsync(int id)
        {
            try
            {
                return await _service.GetNewDetails(id);
            }
            catch (Exception ex) { return null; }
        }

    }
}
