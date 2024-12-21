using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restuarants.Dtos;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Queries.GetFavoriteRestaurants
{
    public class GetFavoriteRestaurantsQueryHandler(ILogger<GetFavoriteRestaurantsQueryHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IUserContext userContext,
        IMapper mapper) : IRequestHandler<GetFavoriteRestaurantsQuery, IEnumerable<RestaurantDto>>
    {
        public async Task<IEnumerable<RestaurantDto>> Handle(GetFavoriteRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation(@"Getting User #{UserId} Favorite restaurants", user.Id);

            var favRestaurants = await restaurantsRepository.GetFavoriteRestaurants(user.Id);
            if (!favRestaurants.Any())
                throw new NotFoundException(nameof(FavoriteRestaurant), user.Id);

            var restaurantIds = favRestaurants.Select(f => f.RestaurantId);

            var restaurants = await restaurantsRepository.GetRestaurantsByIdsAsync(restaurantIds);

            return restaurants.Select(restaurant => mapper.Map<RestaurantDto>(restaurant));
        }
    }
}
