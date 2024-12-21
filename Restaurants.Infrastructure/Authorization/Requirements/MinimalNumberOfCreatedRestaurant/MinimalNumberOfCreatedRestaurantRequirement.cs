using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimalNumberOfCreatedRestaurant
{
    public class MinimalNumberOfCreatedRestaurantRequirement(int minimalNumberOfRestaurants): IAuthorizationRequirement
    {
        public int MinimalNumberOfRestaurants { get; } = minimalNumberOfRestaurants;
    }
}
