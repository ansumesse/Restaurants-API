using MediatR;

namespace Restaurants.Application.Restuarants.Commands.UnFavoriteDish
{
    public class UnFavoriteDishCommand(int restaurantId, int dishId) : IRequest
    {
        public int RestaurantId { get; } = restaurantId;
        public int DishId { get; } = dishId;
    }
}
