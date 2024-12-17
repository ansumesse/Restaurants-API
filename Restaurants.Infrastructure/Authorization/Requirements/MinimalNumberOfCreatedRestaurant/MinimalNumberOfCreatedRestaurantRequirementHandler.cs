using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimalNumberOfCreatedRestaurant
{
    public class MinimalNumberOfCreatedRestaurantRequirementHandler(IUserContext userContext,
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
