using AutoMapper;
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
    internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, 
        ILogger<RestaurantsService> logger,
        IMapper mapper) : IRestaurantsService
    {
        public async Task<int> CreateAsync(CreateRestaurantDto dto)
        {
            logger.LogInformation("Creating restaurant");
            return await restaurantsRepository.CreateAsync(mapper.Map<Restaurant>(dto));
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            logger.LogInformation("Getting all restaurants");
            var restaurants = await restaurantsRepository.GetAllAsync();
            return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }

        public async Task<RestaurantDto> GetRestaurantByIdAsync(int id)
        {
            logger.LogInformation($"Getting restaurant #{id}");
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(id);
            return mapper.Map<RestaurantDto>(restaurant);
        }
    }
}
