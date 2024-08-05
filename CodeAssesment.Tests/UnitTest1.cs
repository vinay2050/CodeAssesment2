using CodeAssesment.API.Controllers;
using CodeAssesment.Service.Interface;
using CodeAssesment.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CodeAssesment.Service;

namespace CodeAssesment.Tests
{
    public class NewsControllerTests
    {
        private readonly Mock<INewsService> _mockNewsService;
        private readonly NewsController _controller;

        public NewsControllerTests()
        {
            _mockNewsService = new Mock<INewsService>();
            _controller = new NewsController(_mockNewsService.Object);
        }

        [Fact]
        public async Task GetStories_ReturnsOkResult_WithListOfNews()
        {
            // Arrange
            var mockNewsData = new List<GetStoriesDataResponse>
            {
                new GetStoriesDataResponse { Title = "Title 1", Url = "http://example.com/1" },
                new GetStoriesDataResponse { Title = "Title 2", Url = "http://example.com/2" }
            };
            var mockNewsResponse = new GetStoriesResponse { Data = mockNewsData };

            _mockNewsService.Setup(service => service.GetNews(null, 1, 10)).ReturnsAsync(mockNewsResponse);

            // Act
            var result = await _controller.GetStories(null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetStoriesResponse>(okResult.Value);
            Assert.Equal(2, returnValue.Data.Count);
        }

        [Fact]
        public async Task GetStories_WithTitleFilter_ReturnsFilteredNews()
        {
            // Arrange
            var mockNewsData = new List<GetStoriesDataResponse>
            {
                new GetStoriesDataResponse { Title = "Title 1", Url = "http://example.com/1" }
            };
            var mockNewsResponse = new GetStoriesResponse { Data = mockNewsData };

            _mockNewsService.Setup(service => service.GetNews("Title 1", 1, 10)).ReturnsAsync(mockNewsResponse);

            // Act
            var result = await _controller.GetStories("Title 1", null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetStoriesResponse>(okResult.Value);
            Assert.Single(returnValue.Data);
            Assert.Equal("Title 1", returnValue.Data[0].Title);
        }

        [Fact]
        public async Task RefreshCache_ReturnsOkResult()
        {
            // Arrange
            _mockNewsService.Setup(service => service.RefereshChache()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RefreshCache();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Success", okResult.Value);
        }
    }
}
