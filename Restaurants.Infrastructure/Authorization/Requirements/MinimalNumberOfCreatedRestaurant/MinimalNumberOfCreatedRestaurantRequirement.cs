using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimalNumberOfCreatedRestaurant
{
    public class MinimalNumberOfCreatedRestaurantRequirement(int minimalNumberOfRestaurants): IAuthorizationRequirement
    {
        public int MinimalNumberOfRestaurants { get; } = minimalNumberOfRestaurants;
    }
}
