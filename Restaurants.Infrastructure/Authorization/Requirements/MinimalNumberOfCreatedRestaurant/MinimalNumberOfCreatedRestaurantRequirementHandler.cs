using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimalNumberOfCreatedRestaurant
{
    internal class MinimalNumberOfCreatedRestaurantRequirementHandler(IUserContext userContext,
        IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<MinimalNumberOfCreatedRestaurantRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalNumberOfCreatedRestaurantRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();

            var restaurants = await restaurantsRepository.GetAllAsync();
            var ownedRestaurantsCount = restaurants.Count(r => r.OwnerId == currentUser.Id);

            if(ownedRestaurantsCount >= requirement.MinimalNumberOfRestaurants)
            {
                context.Succeed(requirement);
            }
            else
                context.Fail();
        }
    }
}
