using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Restaurants.APITests;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers.Tests
{
    public class IdentityControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IUserStore<User>> _userStoreMock = new();

        private HttpClient client;
        public IdentityControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakepolicyEvaluator>();
                    services.AddScoped(typeof(IUserStore<User>), _ => _userStoreMock.Object);
                });
            });

            client = _factory.CreateClient();
        }
        [Fact()]
        public async Task UpdateUserDetails_ForValidRequest_Returns204NoContent()
        {
            // Arrange
            var user = new User()
            {
                Id = "1",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                Nationality = "German"
            };
            _userStoreMock.Setup(u => u.FindByIdAsync("1", default))
                .ReturnsAsync(user);

            // Act
            var result = await client.PatchAsJsonAsync("api/Identity/User", user);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            _userStoreMock.Verify(u => u.UpdateAsync(user, default), Times.Once);
        }
        [Fact()]
        public async Task UpdateUserDetails_ForNonExistingUser_Returns404NotFound()
        {
            // Arrange
            _userStoreMock.Setup(u => u.FindByIdAsync("1", default))
                .ReturnsAsync((User?)null);

            // Act
            var result = await client.PatchAsJsonAsync("api/Identity/User", new {});

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}