using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Commands.FavoriteRestaurant
{
    public class FavoriteRestaurantCommand(int restaurantId) : IRequest
    {
        public int RestaurantId { get; } = restaurantId;
    }
}
