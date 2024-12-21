using MediatR;
using Restaurants.Application.Restuarants.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Queries.GetFavoriteRestaurants
{
    public class GetFavoriteRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
    {
    }
}
