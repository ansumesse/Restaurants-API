using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDish
{
    public class DeleteDishCommand(int restuarantId, int dishId) : IRequest
    {
        public int RestaurantId { get; } = restuarantId;
        public int DishId { get; } = dishId;
    }
}
