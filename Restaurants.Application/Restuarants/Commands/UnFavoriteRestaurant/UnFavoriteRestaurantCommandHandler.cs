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

            var dbFav = await restaurantsRepository.GetFavoriteRestaurant(user.Id, request.RestaurantId);

            if (dbFav is null)
                throw new InvalidOperationException("You don't have this restaurant in you Favorites.");

            await restaurantsRepository.UnFavoriteAsync(dbFav);
        }
    }
}
