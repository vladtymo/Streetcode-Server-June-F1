using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Streetcode.BLL.Services.CookieService.Interfaces;
using Streetcode.BLL.Services.CookieService.Realizations;
using Xunit;

namespace Streetcode.XUnitTest.Services.CookieServiceTests
{
    public class CookieServiceTests
    {
        [Theory]
        [InlineData("test1", "someValue1")]
        [InlineData("test2", "someValue2")]
        [InlineData("test3", "someValue3")]
        public async Task Success_When_AddingInfoToCookies(string key, string value)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var cookieService = CreateService();

            // Act
            await cookieService!.
                AppendCookiesToResponseAsync(httpContext.Response, (key, value, new CookieOptions()));
            
            // Assert
            Assert.True(httpContext.Response.Headers.Values.Contains(string.Format("{0}={1}; path=/", key, value)));
        }

        [Fact]
        public async Task Success_When_ClearCookiesExecuted()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var cookieService = CreateService();

            await cookieService!.AppendCookiesToResponseAsync(httpContext.Response, ("test", "value", new CookieOptions()));

            // Act
            await cookieService.ClearCookiesAsync(httpContext);

            // Assert
            Assert.False(httpContext.Response.Headers.Values.Contains("test=value; path=/"));
        }

        public static ICookieService? CreateService()
        {
            IServiceCollection col = new ServiceCollection();

            col.AddSingleton<ICookieService, CookieService>();

            return col.BuildServiceProvider().GetService<ICookieService>() ?? null;
        }
    }
}
