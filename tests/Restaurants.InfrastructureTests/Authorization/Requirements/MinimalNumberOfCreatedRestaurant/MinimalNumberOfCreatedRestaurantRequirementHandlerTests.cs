using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimalNumberOfCreatedRestaurant.Tests
{
    public class MinimalNumberOfCreatedRestaurantRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
        {
            // Arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Owner], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>
            {
                new Restaurant{ OwnerId = "1"},
                new Restaurant{ OwnerId = "1"},
                new Restaurant{ OwnerId = "2"},
            };
            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirementHandler = new MinimalNumberOfCreatedRestaurantRequirementHandler(userContextMock.Object,
                restaurantRepositoryMock.Object);

            var requirement = new MinimalNumberOfCreatedRestaurantRequirement(2);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // Act
            await requirementHandler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeTrue();
        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
        {
            // Arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Owner], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>
            {
                new Restaurant{ OwnerId = "1"},
                new Restaurant{ OwnerId = "2"},
            };
            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirementHandler = new MinimalNumberOfCreatedRestaurantRequirementHandler(userContextMock.Object,
                restaurantRepositoryMock.Object);

            var requirement = new MinimalNumberOfCreatedRestaurantRequirement(2);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // Act
            await requirementHandler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeFalse();
            context.HasFailed.Should().BeTrue();
        }
    }
}