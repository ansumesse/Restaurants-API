using Restaurants.API.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Restaurants.Domain.Repositories.Restaurants;
using Restaurants.Domain.Repositories.Dishes;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.APITests;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Domain.Entities;
using System.Net.Http.Json;
using Restaurants.Application.Dishes.Dtos;
using FluentAssertions;
using System.Net;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;

namespace Restaurants.API.Controllers.Tests
{
    public class DishesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> restaurantsRepositoryMock = new();
        private readonly Mock<IDishesRepository> dishesRepositoryMock = new();
        HttpClient client;
        public DishesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakepolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => restaurantsRepositoryMock.Object));
                    services.Replace(ServiceDescriptor.Scoped(typeof(IDishesRepository), _ => dishesRepositoryMock.Object));
                });
            });
            client = _factory.CreateClient();
        }
        [Fact()]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                Dishes = [
                    new Dish{Id = 1},
                    new Dish{Id = 2},
                    new Dish{Id = 3}
                    ]
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            // Act
            var result = await client.GetAsync($"api/Restaurant/{1}/Dishes");
            var dishesDto = await result.Content.ReadFromJsonAsync<IEnumerable<DishDto>>();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            dishesDto.Should().NotBeNull();
            dishesDto.Count().Equals(3);
        }
        [Fact()]
        public async Task GetAll_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result = await client.GetAsync($"api/Restaurant/{1}/Dishes");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task GetById_ForExistingDish_Returns200Ok()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                Dishes = [
                    new Dish{Id = 1, Name = "Test Dish"}
                    ]
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            // Act
            var result = await client.GetAsync($"api/Restaurant/{1}/Dishes/1");
            var disheDto = await result.Content.ReadFromJsonAsync<DishDto>();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            disheDto.Id.Should().Be(1);
        }
        [Fact()]
        public async Task GetById_ForNonExistingDish_Returns404NotFound()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            // Act
            var result = await client.GetAsync($"api/Restaurant/{1}/Dishes/1");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task CreateDish_ForValidRequest_Returns201Created()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            var command = new CreateDishCommand()
            {
                Name = "Test",
                Price = 100,
                KiloCalories = 100,
                Description = "Test Description"
            };
            // Act
            var result = await client.PostAsJsonAsync($"api/Restaurant/{1}/Dishes", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact()]
        public async Task CreateDish_ForinvalidDish_Returns400BadRequest()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            var command = new CreateDishCommand()
            {
                Name = "Test"
            };
            // Act
            var result = await client.PostAsJsonAsync($"api/Restaurant/{1}/Dishes", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact()]
        public async Task CreateDish_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            var command = new CreateDishCommand()
            {
                Name = "Test",
                Price = 100,
                KiloCalories = 100,
                Description = "Test Description"
            };

            // Act
            var result = await client.PostAsJsonAsync($"api/Restaurant/{1}/Dishes", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
       
        [Fact()]
        public async Task UpdateDish_ForValidRequest_Returns204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            var oldDish = new Dish
            {
                Id = 1,
                RestaurantId = 1
            };
            dishesRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, oldDish.Id))
                .Returns(oldDish);

            var command = new UpdateDishCommand()
            {
                Price = 100,
                KiloCalories = 100,
                Description = "Test Description"
            };
            // Act
            var result = await client.PatchAsJsonAsync($"api/Restaurant/{1}/Dishes/{1}", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact()]
        public async Task UpdateDish_ForInValidRequest_Returns400BadRequest()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            var oldDish = new Dish
            {
                Id = 1,
                RestaurantId = 1
            };
            dishesRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, oldDish.Id))
                .Returns(oldDish);

            var command = new UpdateDishCommand()
            {
                Price = 100
            };
            // Act
            var result = await client.PatchAsJsonAsync($"api/Restaurant/{1}/Dishes/{1}", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact()]
        public async Task UpdateDish_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            var command = new UpdateDishCommand()
            {
                Price = 100,
                KiloCalories = 100,
                Description = "Test Description"
            };
            // Act
            var result = await client.PatchAsJsonAsync($"api/Restaurant/{1}/Dishes/{1}", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task UpdateDish_ForNonExistingDish_Returns404NotFound()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

     
            dishesRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, 1))
                .Returns((Dish?)null);

            var command = new UpdateDishCommand()
            {
                Price = 100,
                KiloCalories = 100,
                Description = "Test Description"
            };
            // Act
            var result = await client.PatchAsJsonAsync($"api/Restaurant/{1}/Dishes/{1}", command);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task DeleteAll_ForValidRequest_Returns204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                Dishes = [
                   new Dish{Id = 1},
                    new Dish{Id = 2},
                    new Dish{Id = 3}
                   ],
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);
         
            // Act
            var result = await client.DeleteAsync($"api/Restaurant/{1}/Dishes");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact()]
        public async Task DeleteAll_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result = await client.DeleteAsync($"api/Restaurant/{1}/Dishes");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task DeleteDish_ForValidRequest_Returns204NoContent()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);

            var oldDish = new Dish
            {
                Id = 1,
                RestaurantId = 1
            };
            dishesRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, oldDish.Id))
                .Returns(oldDish);
            // Act
            var result = await client.DeleteAsync($"api/Restaurant/{1}/Dishes/{1}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact()]
        public async Task DeleteDish_ForNonExistingRestaurant_Returns404NotFound()
        {
            // Arrange
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync((Restaurant?)null);

            // Act
            var result = await client.DeleteAsync($"api/Restaurant/{1}/Dishes/{1}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact()]
        public async Task DeleteDish_ForNonExistingDish_Returns404NotFound()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                OwnerId = "1"
            };
            restaurantsRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(1))
                .ReturnsAsync(restaurant);


            dishesRepositoryMock.Setup(d => d.GetRestaurantDishById(restaurant, 1))
                .Returns((Dish?)null);

            // Act
            var result = await client.DeleteAsync($"api/Restaurant/{1}/Dishes/{1}");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}