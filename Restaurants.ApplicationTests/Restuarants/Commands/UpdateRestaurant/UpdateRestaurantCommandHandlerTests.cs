using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restuarants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories.Restaurants;
using Xunit;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;
    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _restaurantsRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_RestaurantNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Id = 1 };
        _restaurantsRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(command.Id))
            .ReturnsAsync((Restaurant)null);

        // Act
        Func<Task> func = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await func.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {1} does not exist.");
    }

    [Fact]
    public async Task Handle_ForNotAuthorized_ShouldThrowForbidException()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Id = 1 };
        var restaurant = new Restaurant { Id = 1 };
        _restaurantsRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(command.Id))
            .ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorized(restaurant, ResourceOperation.Update))
            .Returns(false);

        // Act
        Func<Task> func = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await func.Should()
           .ThrowAsync<ForbidException>();
    }

    [Fact]
    public async Task Handle_ForValidRequest_ShouldUpdatesRestaurant()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Id = 1, Name = "Updated Name" };
        var restaurant = new Restaurant { Id = 1, Name = "Old Name" };
        _restaurantsRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(command.Id))
            .ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorized(restaurant, ResourceOperation.Update))
            .Returns(true);
        _mapperMock.Setup(mapper => mapper.Map(command, restaurant))
            .Returns(new Restaurant { Id = 1, Name = "Updated Name" });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _restaurantsRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Restaurant>(r => r.Name == "Updated Name")), Times.Once);
    }
}
