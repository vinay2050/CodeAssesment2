using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAssesment.Service.Interface
{
    public interface INewsService
    {
        Task<GetStoriesResponse> GetNews(string title, int pageNumber, int pageSize);
        Task RefereshChache();
    }
}
