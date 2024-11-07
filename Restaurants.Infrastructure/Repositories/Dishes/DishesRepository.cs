using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Dishes;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories.Dishes
{
    internal class DishesRepository(RestaurantDbContext context) : IDishesRepository
    {
        public async Task CreateAsync(Dish entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }
}
