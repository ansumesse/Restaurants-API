using Xunit;
using Restaurants.API.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Entities;
using FluentAssertions;

namespace Restaurants.API.Middlewares.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

            var Middleware = new ErrorHandlingMiddleware(loggerMock.Object);

            var httpContext = new DefaultHttpContext();
            var nextMock = new Mock<RequestDelegate>();

            // Act
            await Middleware.InvokeAsync(httpContext, nextMock.Object);

            // Assert
            nextMock.Verify(n => n.Invoke(httpContext), Times.Once);
        }
        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

            var Middleware = new ErrorHandlingMiddleware(loggerMock.Object);

            var httpContext = new DefaultHttpContext();

            var notFoundEx = new NotFoundException(nameof(Restaurant), "1");

            // Act
            await Middleware.InvokeAsync(httpContext, _ => throw notFoundEx);

            // Assert
            httpContext.Response.StatusCode.Should().Be(404);
        }
        [Fact()]
        public async Task InvokeAsync_WhenNotForbidExceptionThrown_ShouldSetStatusCode403()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

            var Middleware = new ErrorHandlingMiddleware(loggerMock.Object);

            var httpContext = new DefaultHttpContext();

            var forbidException = new ForbidException();

            // Act
            await Middleware.InvokeAsync(httpContext, _ => throw forbidException);

            // Assert
            httpContext.Response.StatusCode.Should().Be(403);
        }
        [Fact()]
        public async Task InvokeAsync_WhenNotGenericExceptionThrown_ShouldSetStatusCode500()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

            var Middleware = new ErrorHandlingMiddleware(loggerMock.Object);

            var httpContext = new DefaultHttpContext();

            // Act
            await Middleware.InvokeAsync(httpContext, _ => throw new DivideByZeroException());

            // Assert
            httpContext.Response.StatusCode.Should().Be(500);
        }
    }
}