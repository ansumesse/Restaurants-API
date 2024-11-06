using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantHandler(ILogger<DeleteRestaurantHandler> logger,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand, bool>
    {
        public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Removing Restaurant #{RestaurantId}", request.Id);
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);
            if (restaurant is null)
                return false;

            await restaurantsRepository.DeleteAsync(restaurant);
            return true;
        }
    }
}
