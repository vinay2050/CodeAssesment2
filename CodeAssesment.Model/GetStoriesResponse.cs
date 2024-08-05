using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAssesment.Service
{
    public class GetStoriesResponse
    {
        public List<GetStoriesDataResponse> Data { get; set; }
        public int TotalCount { get; set; }
    }

    public class GetStoriesDataResponse
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
