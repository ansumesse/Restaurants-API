using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Dishes;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (dbFav is null)
                throw new InvalidOperationException("You don't have this dish in you Favorites.");

            await restaurantsRepository.UnFavoriteDishAsync(dbFav);
        }
    }
}
