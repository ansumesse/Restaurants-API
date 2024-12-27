using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Application.Restuarants.Commands.FavoriteRestaurant
{
    public class FavoriteRestaurantCommandHandler(ILogger<FavoriteRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IUserContext userContext) : IRequestHandler<FavoriteRestaurantCommand>
    {
        public async Task Handle(FavoriteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation(@"{UserEmail} favorites Restaurant #{RestaurantId}", 
                user.Email, 
                request.RestaurantId);

            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(restaurant), request.RestaurantId.ToString());

            var dbFav = await restaurantsRepository.GetFavoriteRestaurants(user.Id, request.RestaurantId);
            if (dbFav.Any())
                throw new FavoriteAlreadyExistsException(nameof(restaurant));

            var fav = new Domain.Entities.FavoriteRestaurant
            {
                RestaurantId = request.RestaurantId,
                UserId = user.Id,
                FavoritedAt = DateTime.Now
            };

            await restaurantsRepository.FavoriteAsync(fav);
        }
    }
}
