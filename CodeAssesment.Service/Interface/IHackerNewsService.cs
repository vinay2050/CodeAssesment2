using CodeAssesment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAssesment.Service.Interface
{
    public interface IHackerNewsService
    {
        Task<List<int>> GetTopStories();
        Task<HackerStoryDetailsResponse> GetNewDetails(int id);
    }
}
