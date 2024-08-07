using CodeAssesment.Model;
using CodeAssesment.Service.Interface;
using CodeAssesment.Service.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CodeAssesment.Tests
{
    public class NewsServiceTests
    {
        private readonly NewsService _newsService;
        private readonly Mock<IHackerNewsService> _mockHackerNewsService;
        private readonly ConfigeData _configeData;

        public NewsServiceTests()
        {
            _mockHackerNewsService = new Mock<IHackerNewsService>();
            _configeData = new ConfigeData
            {
                NewsData = new List<HackerStoryDetailsResponse>()
            };
            _newsService = new NewsService(_mockHackerNewsService.Object, _configeData);
        }

        [Fact]
        public async Task GetNews_ReturnsFilteredStories_WhenTitleIsProvided()
        {
            // Arrange
            var mockStories = new List<HackerStoryDetailsResponse>
    {
        new HackerStoryDetailsResponse { title = "Title 1", url = "http://example.com/1" },
        new HackerStoryDetailsResponse { title = "Title 2", url = "http://example.com/2" }
    };
            _configeData.NewsData.AddRange(mockStories);

            // Act
            var result = await _newsService.GetNews("Title 1", 1, 10);

            // Assert
            Assert.NotNull(result); // Ensure result is not null
            Assert.NotNull(result.Data); // Ensure data is not null
            Assert.Single(result.Data); // Ensure only one story is returned
            Assert.Equal("Title 1", result.Data.First().Title);
        }


        [Fact]
        public async Task GetNews_ReturnsPagedStories()
        {
            // Arrange
            var mockStories = Enumerable.Range(1, 20).Select(i =>
                new HackerStoryDetailsResponse { title = $"Title {i}", url = $"http://example.com/{i}" }).ToList();
            _configeData.NewsData.AddRange(mockStories);

            // Act
            var result = await _newsService.GetNews(null, 2, 5);

            // Assert
            Assert.NotNull(result); // Ensure result is not null
            Assert.NotNull(result.Data); // Ensure data is not null
            Assert.Equal(5, result.Data.Count); // Ensure 5 stories are returned
            Assert.Equal("Title 6", result.Data.First().Title);
        }


        [Fact]
        public async Task RefereshChache_PopulatesNewsData()
        {
            // Arrange
            var newsIds = new List<int> { 1, 2 };
            var mockStories = new List<HackerStoryDetailsResponse>
            {
                new HackerStoryDetailsResponse { title = "Title 1", url = "http://example.com/1" },
                new HackerStoryDetailsResponse { title = "Title 2", url = "http://example.com/2" }
            };
            _mockHackerNewsService.Setup(service => service.GetTopStories()).ReturnsAsync(newsIds);
            _mockHackerNewsService.Setup(service => service.GetNewDetails(1)).ReturnsAsync(mockStories[0]);
            _mockHackerNewsService.Setup(service => service.GetNewDetails(2)).ReturnsAsync(mockStories[1]);

            // Act
            await _newsService.RefereshChache();

            // Assert
            Assert.Equal(2, _configeData.NewsData.Count);
            Assert.Contains(_configeData.NewsData, story => story.title == "Title 1");
            Assert.Contains(_configeData.NewsData, story => story.title == "Title 2");
        }

        [Fact]
        public async Task RefereshChache_HandlesExceptions()
        {
            // Arrange
            var newsIds = new List<int> { 1, 2 };
            _mockHackerNewsService.Setup(service => service.GetTopStories()).ReturnsAsync(newsIds);
            _mockHackerNewsService.Setup(service => service.GetNewDetails(It.IsAny<int>())).ThrowsAsync(new Exception());

            // Act
            await _newsService.RefereshChache();

            // Assert
            Assert.Empty(_configeData.NewsData);
        }
    }
}
