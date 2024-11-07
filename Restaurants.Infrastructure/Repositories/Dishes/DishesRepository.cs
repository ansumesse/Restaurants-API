﻿using Restaurants.Domain.Entities;
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
        public async Task<int> CreateAsync(Dish entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task DeleteAllAsync(IEnumerable<Dish> entities)
        {
            context.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
    }
}