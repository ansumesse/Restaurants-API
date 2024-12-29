﻿using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.APITests;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Dishes;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.API.Controllers.Tests
{
    public class FavoritesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> restaurantsRepositoryMock = new();
        private readonly Mock<IDishesRepository> dishsRepositoryMock = new();
        private HttpClient client;
        public FavoritesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(servicesConfiguration =>
                {
                    servicesConfiguration.AddSingleton<IPolicyEvaluator, FakepolicyEvaluator>();
                    servicesConfiguration.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                        _ => restaurantsRepositoryMock.Object));
                    servicesConfiguration.Replace(ServiceDescriptor.Scoped(typeof(IDishesRepository),
                        _ => dishsRepositoryMock.Object));
                });
            });

            client = _factory.CreateClient();
        }

        [Fact()]
        public async Task GetFavoriteRestaurants_ForValidRequest_Returns200Ok()
        {
            // Arrange
            var favourateRestaurants = new List<FavoriteRestaurant>()
            {
                new FavoriteRestaurant() { Id = 1 },
                new FavoriteRestaurant() { Id = 2 },
                new FavoriteRestaurant() { Id = 3 },
            };
            restaurantsRepositoryMock.Setup(r => r.GetFavoriteRestaurants("1", null))
                .ReturnsAsync(favourateRestaurants);

            // Act
            var result = await client.GetAsync("api/Favorites/Restuarant");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        } 
        [Fact()]
        public async Task GetFavoriteRestaurants_ForNonExistingFavoriteRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetFavoriteRestaurants("1", null))
                .ReturnsAsync(new List<FavoriteRestaurant>());

            // Act
            var result = await client.GetAsync("api/Favorites/Restuarant");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}