using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories.Restaurants
{
    internal class RestaurantsRepository(RestaurantDbContext context) : IRestaurantsRepository
    {
        public async Task<int> CreateAsync(Restaurant entity)
        {
            await context.Restaurants.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await context.Restaurants.ToListAsync();
        }
        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            return await context.Restaurants.FindAsync(id);
        }
    }
}
