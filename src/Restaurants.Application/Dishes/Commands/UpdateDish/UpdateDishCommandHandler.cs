using AutoMapper;
using MediatR;
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

namespace Restaurants.Application.Dishes.Commands.UpdateDish
{
    public class UpdateDishCommandHandler(ILogger<UpdateDishCommandHandler> logger,
        IDishesRepository dishesRepository,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        IMapper mapper) : IRequestHandler<UpdateDishCommand>
    {
        public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Update Restaurant #{RestaurantId} dish #{DishId} with {@Dish}", request.RestaurantId, request.DishId, request);

            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(!restaurantAuthorizationService.Authorized(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            var dbDish = dishesRepository.GetRestaurantDishById(restaurant, request.DishId);
            if (dbDish is null)
                throw new NotFoundException(nameof(Dish), request.DishId.ToString());

            await dishesRepository.UpdateAsync(mapper.Map(request, dbDish));
        }
    }
}
