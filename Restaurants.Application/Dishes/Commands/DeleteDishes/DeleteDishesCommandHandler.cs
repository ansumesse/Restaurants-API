﻿using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories.Dishes;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes
{
    public class DeleteDishesCommandHandler(ILogger<DeleteDishesCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishesCommand>
    {
        public async Task Handle(DeleteDishesCommand request, CancellationToken cancellationToken)
        {
            logger.LogWarning("Remove dishes for restaurant: {restaurantId}", request.RestaurantId);

            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!restaurantAuthorizationService.Authorized(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            await dishesRepository.DeleteAllAsync(restaurant.Dishes);
        }
    }
}
