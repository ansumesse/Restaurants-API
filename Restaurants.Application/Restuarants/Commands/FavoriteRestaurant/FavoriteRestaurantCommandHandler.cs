using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var dbFav = await restaurantsRepository.GetFavoriteRestaurant(user.Id, request.RestaurantId);
            if (dbFav != null)
                throw new InvalidOperationException("You have already favorited this restaurant.");

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
