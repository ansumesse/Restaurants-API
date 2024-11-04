using Microsoft.Extensions.Logging;
using Restaurants.Application.Restuarants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Services
{
    internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger) : IRestaurantsService
    {
        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            logger.LogInformation("Getting all restaurants");
            var restaurants = await restaurantsRepository.GetAllAsync();
            return null;
        }

        public async Task<RestaurantDto> GetRestaurantByIdAsync(int id)
        {
            logger.LogInformation($"Getting restaurant #{id}");
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(id);
            return null;
        }
    }
}
