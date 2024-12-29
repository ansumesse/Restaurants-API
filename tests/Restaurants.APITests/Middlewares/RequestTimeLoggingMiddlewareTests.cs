using System.Diagnostics;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Restaurants.API.Middlewares.Tests
{
    public class RequestTimeLoggingMiddlewareTests
    {
        private Mock<ILogger<RequestTimeLoggingMiddleware>> _loggerMock;
        private RequestTimeLoggingMiddleware _middleware;
        private HttpContext _httpContext;
        private RequestDelegate _next;
        private Mock<Stopwatch> _stopWatchMock;

        public RequestTimeLoggingMiddlewareTests()
        {
            _loggerMock = new();
            _middleware = new(_loggerMock.Object);
            _httpContext = new DefaultHttpContext();
        }
        [Fact()]
        public async Task InvokeAsync_WhenTimeTookMoreThan4Secondes_ShouldLogInformation()
        {
            // Arrange
            _next = new RequestDelegate(async _ =>
            {
                await Task.Delay(5000);
            });

            // Act
            await _middleware.InvokeAsync(_httpContext, _next);

            // Assert
            _loggerMock.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Once);
        }
        [Fact()]
        public async Task InvokeAsync_WhenTimeTookLessThan4Secondes_ShouldNotLogInformation()
        {
            // Arrange
            _next = new RequestDelegate(async _ =>
            {
                await Task.Delay(2000);
            });

            // Act
            await _middleware.InvokeAsync(_httpContext, _next);

            // Assert
            _loggerMock.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Never);
        }
    }
}