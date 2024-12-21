using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Application.Restuarants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantHandler(ILogger<DeleteRestaurantHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Removing Restaurant #{RestaurantId}", request.Id);
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            if (!restaurantAuthorizationService.Authorized(restaurant, ResourceOperation.Delete))
                throw new ForbidException();

            await restaurantsRepository.DeleteAsync(restaurant);
        }
    }
}
