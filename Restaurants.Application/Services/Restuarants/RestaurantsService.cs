using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Services.Restuarants
{
    internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger) : IRestaurantsService
    {
        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            logger.LogInformation("Getting all restaurants");
            return await restaurantsRepository.GetAllAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            logger.LogInformation($"Getting restaurant #{id}");
            return await restaurantsRepository.GetRestaurantByIdAsync(id);
        }
    }
}
