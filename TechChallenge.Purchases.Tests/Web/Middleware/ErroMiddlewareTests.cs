using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TechChallenge.Purchases.Web.Middleware;
using Xunit;

namespace TechChallenge.Purchases.Tests.Web.Middleware
{
    public class ErroMiddlewareTests
    {
        [Fact]
        public async Task DeveRetornar500_QuandoOcorreExcecao()
        {
            // Arrange
            var middleware = new ErroMiddleware(context =>
            {
                throw new Exception("Test exception");
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var body = await reader.ReadToEndAsync();

            var expectedResponse = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred. Please try again later."
            };
            var expectedJson = JsonSerializer.Serialize(expectedResponse);

            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);
            Assert.Equal(expectedJson, body);
        }

        [Fact]
        public async Task DeveChamarProximoDelegate_QuandoNaoOcorreExcecao()
        {
            // Arrange
            var nextCalled = false;
            var middleware = new ErroMiddleware(context =>
            {
                nextCalled = true;
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return Task.CompletedTask;
            });

            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(nextCalled);
            Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
        }
    }
}