using CodeAssesment.Model;
using CodeAssesment.Service.Interface;
using CodeAssesment.Service.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class HackerNewsServiceTests
{
    private readonly Mock<HttpClient> _mockHttpClient;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly HackerNewsService _hackerNewsService;

    public HackerNewsServiceTests()
    {
        _mockHttpClient = new Mock<HttpClient>();
        _mockConfiguration = new Mock<IConfiguration>();

        _mockConfiguration.Setup(config => config["HackerTopStoryUrl"]).Returns("https://example.com/topstories");
        _mockConfiguration.Setup(config => config["HackerStoryDetailsUrl"]).Returns("https://example.com/stories/{0}");

        _hackerNewsService = new HackerNewsService(_mockHttpClient.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task GetTopStories_ReturnsListOfIds()
    {
        // Arrange
        var mockResponse = new List<int> { 1, 2, 3 };
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(mockResponse)
            });

        var httpClient = new HttpClient(httpMessageHandler.Object);

        var hackerNewsService = new HackerNewsService(httpClient, _mockConfiguration.Object);

        // Act
        var result = await hackerNewsService.GetTopStories();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(mockResponse, result);
    }

    [Fact]
    public async Task GetNewDetails_ReturnsStoryDetails()
    {
        // Arrange
        var mockStory = new HackerStoryDetailsResponse { title = "Title 1", url = "http://example.com/1" };
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(mockStory)
            });

        var httpClient = new HttpClient(httpMessageHandler.Object);

        var hackerNewsService = new HackerNewsService(httpClient, _mockConfiguration.Object);

        // Act
        var result = await hackerNewsService.GetNewDetails(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mockStory.title, result.title);
        Assert.Equal(mockStory.url, result.url);
    }
}
