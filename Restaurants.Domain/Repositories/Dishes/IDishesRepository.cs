using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Repositories.Dishes
{
    public interface IDishesRepository
    {
        public Dish? GetRestaurantDishById(Restaurant restaurant, int dishId);
        Task<int> CreateAsync(Dish entity);
        public Task UpdateAsync(Dish entity);
        Task DeleteAllAsync(IEnumerable<Dish> entities);
    }
}
