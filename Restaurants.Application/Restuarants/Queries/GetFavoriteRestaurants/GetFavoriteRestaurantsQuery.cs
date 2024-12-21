using MediatR;
using Restaurants.Application.Restuarants.Dtos;

namespace Restaurants.Application.Restuarants.Queries.GetFavoriteRestaurants
{
    public class GetFavoriteRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
    {
    }
}
