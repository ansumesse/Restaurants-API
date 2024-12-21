using MediatR;
using Restaurants.Application.Restuarants.Dtos;

namespace Restaurants.Application.Restuarants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery(int id) : IRequest<RestaurantDto>
    {
        public int Id { get; } = id;
    }
}
