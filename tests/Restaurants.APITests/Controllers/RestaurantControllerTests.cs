using Restaurants.API.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Moq;
using Restaurants.Domain.Repositories.Restaurants;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.APITests;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Application.Restuarants.Dtos;
using Restaurants.Domain.Entities;
using System.Net.Http.Json;
using System.Net;
using Restaurants.Application.Restuarants.Commands.CreateRestaurant;
using Restaurants.Application.Restuarants.Commands.UpdateRestaurant;

namespace Restaurants.API.Controllers.Tests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
        HttpClient client;
        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakepolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                                                _ => _restaurantsRepositoryMock.Object));
                });
            });

            client = _factory.CreateClient();
        }

        [Fact()]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // Arrange

            // Act
            var result = await client.GetAsync("api/Restaurant");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
        {
            // arrange
            var id = 1123;
            _restaurantsRepositoryMock.Setup(m => m.GetRestaurantByIdAsync(id)).ReturnsAsync((Restaurant?)null);

            // act
            var response = await client.GetAsync($"/api/restaurant/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ForExistingId_ShouldReturn200Ok()
        {
            // arrange
            var id = 99;
            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test",
                Description = "Test description"
            };
            _restaurantsRepositoryMock.Setup(m => m.GetRestaurantByIdAsync(id)).ReturnsAsync(restaurant);

            // act
            var response = await client.GetAsync($"/api/restaurant/{id}");
            var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Name.Should().Be("Test");
            restaurantDto.Description.Should().Be("Test description");
        }

        [Fact()]
        public async Task Create_ValidRestaurant_ShouldReturn201Created()
        {
            // Arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Test",
                Description = "test",
                Category = "Italian",
                ContactEmail = "test@test.com",
                PostalCode = "22-345"
            };


            // Act
            var response = await client.PostAsJsonAsync("/api/restaurant", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact()]
        public async Task Create_InValidRestaurant_ShouldReturn400BadRequest()
        {
            // Arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Test",
                Description = "test",
                Category = "Egyptian",
                ContactEmail = "test@test.com",
                PostalCode = "22-345"
            };


            // Act
            var response = await client.PostAsJsonAsync("/api/restaurant", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact()]
        public async Task Update_ValidRestaurant_ShouldReturn204NoContent()
        {
            // Arrange
            var command = new UpdateRestaurantCommand()
            {
                Name = "Test",
                Description = "test",
            };

            var restaurant = new Restaurant { Id = 1, Name = "Old Name", OwnerId = "1" };
            _restaurantsRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            // Act
            var result = await client.PatchAsJsonAsync($"/api/restaurant/{restaurant.Id}", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task Update_InValidRestaurant_ShouldReturn400BadRequest()
        {
            // Arrange
            var command = new UpdateRestaurantCommand()
            {
                Name = "Te",
                Description = "test",
            };

            var restaurant = new Restaurant { Id = 1, Name = "Old Name", OwnerId = "1" };
            _restaurantsRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            // Act
            var result = await client.PatchAsJsonAsync($"/api/restaurant/{restaurant.Id}", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Update_NonExistingRestaurant_ShouldReturn404NotFound()
        {
            // Arrange
            var command = new UpdateRestaurantCommand()
            {
                Name = "Test",
                Description = "test",
            };

            _restaurantsRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result = await client.PatchAsJsonAsync($"/api/restaurant/{1}", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task Delete_ExistingRestaurant_ShouldReturn204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
                Name = "Test",
                Description = "Test description"
            };
            _restaurantsRepositoryMock.Setup(m => m.GetRestaurantByIdAsync(1)).ReturnsAsync(restaurant);

            // Act
            var result = await client.DeleteAsync($"/api/restaurant/{1}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task Delete_NonExistingRestaurant_ShouldReturn404NotFound()
        {
            // Arrange
            _restaurantsRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result = await client.DeleteAsync($"/api/restaurant/{1}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}