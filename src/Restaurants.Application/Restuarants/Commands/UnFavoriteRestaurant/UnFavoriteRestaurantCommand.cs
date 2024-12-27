using MediatR;

namespace Restaurants.Application.Restuarants.Commands.UnFavoriteRestaurant
{
    public class UnFavoriteRestaurantCommand(int restaurantId) : IRequest
    {
        public int RestaurantId { get; } = restaurantId;
    }
}
