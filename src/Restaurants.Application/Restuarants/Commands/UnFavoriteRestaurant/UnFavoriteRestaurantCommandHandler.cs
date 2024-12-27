using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Application.Restuarants.Commands.UnFavoriteRestaurant
{
    public class UnFavoriteRestaurantCommandHandler(ILogger<UnFavoriteRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IUserContext userContext) : IRequestHandler<UnFavoriteRestaurantCommand>
    {
        public async Task Handle(UnFavoriteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation(@"{UserEmail} Unfavorites Restaurant #{RestaurantId}",
                user.Email,
                request.RestaurantId);

            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(restaurant), request.RestaurantId.ToString());

            var dbFav = await restaurantsRepository.GetFavoriteRestaurants(user.Id, request.RestaurantId);

            if (!dbFav.Any())
                throw new FavoriteNotFoundException(nameof(restaurant), request.RestaurantId.ToString());

            await restaurantsRepository.UnFavoriteAsync(dbFav.First());
        }
    }
}
