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
        public Dish? GetRestaurantDishById(Restaurant restaurant, int dishId)
        {
            return restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
        }
        public async Task<int> CreateAsync(Dish entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(Dish entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
        public async Task DeleteDish(Dish entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(IEnumerable<Dish> entities)
        {
            context.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
    }
}
