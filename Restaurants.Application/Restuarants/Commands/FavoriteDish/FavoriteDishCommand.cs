using MediatR;

namespace Restaurants.Application.Restuarants.Commands.FavoriteDish
{
    public class FavoriteDishCommand(int restaurantId, int dishId) : IRequest
    {
        public int RestaurantId { get; } = restaurantId;
        public int DishId { get; } = dishId;
    }
}
