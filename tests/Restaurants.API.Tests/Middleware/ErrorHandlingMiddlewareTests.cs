using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Middleware.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact()]
        public async void InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();
            await middleware.InvokeAsync(context, nextDelegateMock.Object);

            nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
        }

        [Fact()]
        public async void InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCodeTo404()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var notFoundException = new NotFoundException(nameof(Restaurant), "1");

            await middleware.InvokeAsync(context, _ => throw notFoundException);

            context.Response.StatusCode.Should().Be(404);
        }

        [Fact()]
        public async void InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCodeTo403()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var forbidException = new ForbidException();

            await middleware.InvokeAsync(context, _ => throw forbidException);

            context.Response.StatusCode.Should().Be(403);
        }

        [Fact()]
        public async void InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCodeTo500()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new Exception();

            await middleware.InvokeAsync(context, _ => throw exception);

            context.Response.StatusCode.Should().Be(500);
        }
    }
}