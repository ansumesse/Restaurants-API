using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Restaurants.APITests;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers.Tests
{
    public class IdentityControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IUserStore<User>> _userStoreMock = new();
        private readonly Mock<UserManager<User>> _userManagerMock = new(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);

        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock = new (
            new Mock<IRoleStore<IdentityRole>>().Object,
            new IRoleValidator<IdentityRole>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

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
                    services.AddScoped(typeof(UserManager<User>), _ => _userManagerMock.Object);
                    services.AddScoped(typeof(RoleManager<IdentityRole>), _ => _roleManagerMock.Object);
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
            };
            _userStoreMock.Setup(u => u.FindByIdAsync("1", default))
                .ReturnsAsync(user);

            var command = new UpdateUserDetailsCommand
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                Nationality = "German"
            };
            // Act
            var result = await client.PatchAsJsonAsync("api/Identity/User", command);

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

        [Fact()]
        public async Task AssignUserRole_ForValidRequest_Returns204NoContent()
        {
            // Arrange
            var user = new User()
            {
                Id = "1",
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com"))
                .ReturnsAsync(user);

            var role = new IdentityRole(UserRoles.Admin);
            _roleManagerMock.Setup(r => r.FindByNameAsync("Admin"))
                .ReturnsAsync(role);

            var command = new AssignUserRoleCommand()
            {
                Email = "test@test.com",
                Role = "Admin"
            };

            // Act
            var result = await client.PostAsJsonAsync("api/Identity/UserRole", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            _userManagerMock.Verify(u => u.AddToRoleAsync(user, role.Name), Times.Once);
        }
        [Fact()]
        public async Task AssignUserRole_ForInValidRequest_Returns400BadRequest()
        {
            // Arrange
            var user = new User()
            {
                Id = "1",
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com"))
                .ReturnsAsync(user);

            var role = new IdentityRole(UserRoles.Admin);
            _roleManagerMock.Setup(r => r.FindByNameAsync("Admin"))
                .ReturnsAsync(role);

            var command = new AssignUserRoleCommand()
            {
                Role = "Admin"
            };

            // Act
            var result = await client.PostAsJsonAsync("api/Identity/UserRole", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact()]
        public async Task AssignUserRole_ForNonExistingUser_Returns404NotFound()
        {
            // Arrange
            _userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com"))
                .ReturnsAsync((User?)null);

            var role = new IdentityRole(UserRoles.Admin);
            _roleManagerMock.Setup(r => r.FindByNameAsync("Admin"))
                .ReturnsAsync(role);

            var command = new AssignUserRoleCommand()
            {
                Email = "test@test.com",
                Role = "Admin"
            };

            // Act
            var result = await client.PostAsJsonAsync("api/Identity/UserRole", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task AssignUserRole_ForNonExistingRole_Returns204NoContent()
        {
            // Arrange
            var user = new User()
            {
                Id = "1",
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com"))
                .ReturnsAsync(user);

            _roleManagerMock.Setup(r => r.FindByNameAsync("Admin"))
                .ReturnsAsync((IdentityRole?)null);

            var command = new AssignUserRoleCommand()
            {
                Email = "test@test.com",
                Role = "Admin"
            };

            // Act
            var result = await client.PostAsJsonAsync("api/Identity/UserRole", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


    }
}