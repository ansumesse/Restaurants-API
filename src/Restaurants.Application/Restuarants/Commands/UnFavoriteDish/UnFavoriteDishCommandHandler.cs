﻿using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Dishes;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Application.Restuarants.Commands.UnFavoriteDish
{
    public class UnFavoriteDishCommandHandler(ILogger<UnFavoriteDishCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IUserContext userContext) : IRequestHandler<UnFavoriteDishCommand>
    {
        public async Task Handle(UnFavoriteDishCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("{UserEmail} is unfavoriting Dish #{DishId} from Restaurant #{RestaurantId}",
                user.Email,
                request.DishId,
                request.RestaurantId);

            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(restaurant), request.RestaurantId.ToString());

            var dish = dishesRepository.GetRestaurantDishById(restaurant, request.DishId);
            if (dish is null)
                throw new NotFoundException(nameof(dish), request.DishId.ToString());

            var dbFav = await restaurantsRepository.GetFavoriteDish(user.Id, request.RestaurantId, request.DishId);

            if (dbFav is null)
                throw new FavoriteNotFoundException(nameof(dish), request.DishId.ToString());

            await restaurantsRepository.UnFavoriteDishAsync(dbFav);
        }
    }
}
