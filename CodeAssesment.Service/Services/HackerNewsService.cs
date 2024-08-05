using CodeAssesment.Model;
using CodeAssesment.Service.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CodeAssesment.Service.Services
{
    public class HackerNewsService: IHackerNewsService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public HackerNewsService(HttpClient client, IConfiguration config) 
        {
            _client = client;
            _config = config;
        }

        public async Task<List<int>> GetTopStories()
        {
            List<int> ids=new List<int>();
            try
            {
                string apiUrl = _config["HackerTopStoryUrl"];
                var response = await _client.GetAsync(apiUrl);
                if (response != null && response.IsSuccessStatusCode)
                {
                    ids = await response.Content.ReadFromJsonAsync<List<int>>();
                }
            }
            catch (Exception ex)
            {
            }
            return ids;
        }


        public async Task<HackerStoryDetailsResponse> GetNewDetails(int id)
        {
            HackerStoryDetailsResponse data=null;
            try
            {
                string apiUrl = string.Format( _config["HackerStoryDetailsUrl"], id);
                var response = await _client.GetAsync(apiUrl);
                if (response != null && response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadFromJsonAsync<HackerStoryDetailsResponse>();
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

    }
}
