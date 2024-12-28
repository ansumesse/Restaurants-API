﻿using Restaurants.API.Controllers;
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

namespace Restaurants.API.Controllers.Tests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
        HttpClient client ;
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

    }
}