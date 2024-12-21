using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restuarants.Dtos;
using Restaurants.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQuery : PaginatedQuery<RestaurantDto>
    {
    }
}
