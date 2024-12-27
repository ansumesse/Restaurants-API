using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories.Dishes
{
    public interface IDishesRepository
    {
        public Dish? GetRestaurantDishById(Restaurant restaurant, int dishId);
        Task<int> CreateAsync(Dish entity);
        public Task UpdateAsync(Dish entity);
        Task DeleteAllAsync(IEnumerable<Dish> entities);
        Task DeleteDish(Dish entity);
    }
}
