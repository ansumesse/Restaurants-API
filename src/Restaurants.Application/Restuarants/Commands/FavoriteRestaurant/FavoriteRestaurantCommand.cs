using MediatR;

namespace Restaurants.Application.Restuarants.Commands.FavoriteRestaurant
{
    public class FavoriteRestaurantCommand(int restaurantId) : IRequest
    {
        public int RestaurantId { get; } = restaurantId;
    }
}
