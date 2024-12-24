using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using Xunit;

namespace Restaurants.Application.Restuarants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
        {
            // Arrange
            var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantsRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Restaurant>()))
                .ReturnsAsync(1);

            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

            var command = new CreateRestaurantCommand();
            var restaurant = new Restaurant();
            var imapperMock = new Mock<IMapper>();
            imapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

            var currentUser = new CurrentUser("Owner-Id", "test@test.com", [UserRoles.Admin], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var commandHandler = new CreateRestaurantCommandHandler(
                restaurantsRepositoryMock.Object,
                loggerMock.Object,
                imapperMock.Object,
                userContextMock.Object);

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            restaurant.OwnerId.Should().Be(currentUser.Id);
            restaurantsRepositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once);
        }
    }
}