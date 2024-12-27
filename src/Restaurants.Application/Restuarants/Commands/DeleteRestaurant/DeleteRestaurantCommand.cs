using MediatR;

namespace Restaurants.Application.Restuarants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand(int id) : IRequest
    {
        public int Id { get; } = id;
    }
}
