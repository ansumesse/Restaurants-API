﻿using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Dishes;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Application.Restuarants.Commands.FavoriteDish
{
    public class FavoriteDishCommandHandler(ILogger<FavoriteDishCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IUserContext userContext) : IRequestHandler<FavoriteDishCommand>
    {
        public async Task Handle(FavoriteDishCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation(@"{UserEmail} favorites Restaurant #{RestaurantId} Dish #{DishId}",
                user.Email,
                request.RestaurantId,
                request.DishId);

            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(restaurant), request.RestaurantId.ToString());

            var dish = dishesRepository.GetRestaurantDishById(restaurant, request.DishId);
            if (dish is null)
                throw new NotFoundException(nameof(dish), request.DishId.ToString());

            var dbFav = await restaurantsRepository.GetFavoriteDish(user.Id, request.RestaurantId, request.DishId);
            if (dbFav != null)
                throw new FavoriteAlreadyExistsException(nameof(Dish));

            var fav = new Domain.Entities.FavoriteDish
            {
                RestaurantId = request.RestaurantId,
                UserId = user.Id,
                DishId = request.DishId,
                FavoritedAt = DateTime.Now
            };

            await restaurantsRepository.FavoriteDishAsync(fav);
        }
    }
}
