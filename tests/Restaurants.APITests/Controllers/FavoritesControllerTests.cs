using Restaurants.API.Controllers;
using System.Net;
using System.Net.Http.Json;
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

        [Fact()]
        public async Task FavoriteRestaurant_ForExistingOne_Returns204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };

            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            // Act
            var result = await client.PostAsJsonAsync($"/api/Favorites/Restaurant/{restaurant.Id}", new { });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            restaurantsRepositoryMock.Verify(r => r.FavoriteAsync(It.IsAny<FavoriteRestaurant>()), Times.Once);
        }
        [Fact()]
        public async Task FavoriteRestaurant_ForNonExistingOne_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result = await client.PostAsJsonAsync($"/api/Favorites/Restaurant/{1}", new { });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task FavoriteRestaurant_ForExistingFavoriteRestaurant_Returns405MethodNotAllowed()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };

            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            var favouriteRestaurants = new List<FavoriteRestaurant>
            {
                new FavoriteRestaurant() { Id = 1, RestaurantId = 1, UserId = "1" }
            };
            restaurantsRepositoryMock.Setup(r => r.GetFavoriteRestaurants("1", 1))
                .ReturnsAsync(favouriteRestaurants);

            // Act
            var result = await client.PostAsJsonAsync($"/api/Favorites/Restaurant/{1}", new { });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        [Fact()]
        public async Task UnFavoriteRestaurant_ForExistingOne_Returns204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };

            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            var favouriteRestaurants = new List<FavoriteRestaurant>
            {
                new FavoriteRestaurant() { Id = 1, RestaurantId = 1, UserId = "1" }
            };
            restaurantsRepositoryMock.Setup(r => r.GetFavoriteRestaurants("1", 1))
                .ReturnsAsync(favouriteRestaurants);

            // Act
            var result = await client.DeleteAsync($"/api/Favorites/Restaurant/{restaurant.Id}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            restaurantsRepositoryMock.Verify(r => r.UnFavoriteAsync(It.IsAny<FavoriteRestaurant>()), Times.Once);
        }
        [Fact()]
        public async Task UnFavoriteRestaurant_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            var favouriteRestaurants = new List<FavoriteRestaurant>
            {
                new FavoriteRestaurant() { Id = 1, RestaurantId = 1, UserId = "1" }
            };
            restaurantsRepositoryMock.Setup(r => r.GetFavoriteRestaurants("1", 1))
                .ReturnsAsync(favouriteRestaurants);

            // Act
            var result = await client.DeleteAsync($"/api/Favorites/Restaurant/1");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task UnFavoriteRestaurant_ForNonExistingFavoriteRestaurant_Returns404NotFound()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };

            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            restaurantsRepositoryMock.Setup(r => r.GetFavoriteRestaurants("1", 1))
                .ReturnsAsync(new List<FavoriteRestaurant>());

            // Act
            var result = await client.DeleteAsync($"/api/Favorites/Restaurant/{restaurant.Id}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task FavoriteDish_ForExistingOne_Returns204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            var dish = new Dish()
            {
                Id = 1
            };
            dishsRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, dish.Id))
                .Returns(dish);

            restaurantsRepositoryMock.Setup(r => r.GetFavoriteDish("1", 1, 1))
                .ReturnsAsync((FavoriteDish?)null);

            // Act
            var result =
                await client.PostAsJsonAsync($"/api/Favorites/Restaurant/{restaurant.Id}/Dish/{dish.Id}", new { });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            restaurantsRepositoryMock.Verify(r => r.FavoriteDishAsync(It.IsAny<FavoriteDish>()), Times.Once);
        }
        [Fact()]
        public async Task FavoriteDish_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result =
                await client.PostAsJsonAsync($"/api/Favorites/Restaurant/1/Dish/1", new { });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task FavoriteDish_ForNonExistingDish_Returns404NotFound()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            dishsRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, 1))
                .Returns((Dish?)null);

            // Act
            var result =
                await client.PostAsJsonAsync($"/api/Favorites/Restaurant/{restaurant.Id}/Dish/1", new { });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task FavoriteDish_ForAlreadyFavoriteDish_Returns405MethodNotAllowed()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            var dish = new Dish()
            {
                Id = 1
            };
            dishsRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, dish.Id))
                .Returns(dish);

            restaurantsRepositoryMock.Setup(r => r.GetFavoriteDish("1", 1, 1))
                .ReturnsAsync(new FavoriteDish(){Id = 1});

            // Act
            var result =
                await client.PostAsJsonAsync($"/api/Favorites/Restaurant/{restaurant.Id}/Dish/{dish.Id}", new { });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        [Fact()]
        public async Task UnFavoriteDish_ForExistingOne_Returns204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            var dish = new Dish()
            {
                Id = 1
            };
            dishsRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, dish.Id))
                .Returns(dish);

            restaurantsRepositoryMock.Setup(r => r.GetFavoriteDish("1", 1, 1))
                .ReturnsAsync(new FavoriteDish() { Id = 1 });

            // Act
            var result =
                await client.DeleteAsync($"/api/Favorites/Restaurant/{restaurant.Id}/Dish/{dish.Id}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            restaurantsRepositoryMock.Verify(r => r.UnFavoriteDishAsync(It.IsAny<FavoriteDish>()), Times.Once);
        }
        [Fact()]
        public async Task UnFavoriteDish_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result =
                await client.DeleteAsync($"/api/Favorites/Restaurant/1/Dish/1");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task UnFavoriteDish_ForNonExistingDish_Returns404NotFound()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            dishsRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, 1))
                .Returns((Dish?)null);

            // Act
            var result =
                await client.DeleteAsync($"/api/Favorites/Restaurant/{restaurant.Id}/Dish/1");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task UnFavoriteDish_ForAlreadyNonFavoriteDish_Returns404NotFound()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = 1,
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);

            var dish = new Dish()
            {
                Id = 1
            };
            dishsRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, dish.Id))
                .Returns(dish);

            restaurantsRepositoryMock.Setup(r => r.GetFavoriteDish("1", 1, 1))
                .ReturnsAsync((FavoriteDish?)null);

            // Act
            var result =
                await client.DeleteAsync($"/api/Favorites/Restaurant/{restaurant.Id}/Dish/{dish.Id}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


    }
}