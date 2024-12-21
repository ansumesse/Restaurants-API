using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant
{
    public class GetDishByIdForRestaurantQueryHandler(ILogger<GetDishesForRestaurantQueryHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
    {
        public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieve dish: {dishId} for restaurant: {restaurantId}",request.DishId , request.RestaurantId);

            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
            if (dish is null)
                throw new NotFoundException(nameof(Dish), request.DishId.ToString());

            return mapper.Map<DishDto>(dish);
        }
    }
}
