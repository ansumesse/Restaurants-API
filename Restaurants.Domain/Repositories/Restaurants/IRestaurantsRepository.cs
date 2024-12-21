using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Repositories.Restaurants
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchedPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task<int> CreateAsync(Restaurant entity);
        Task DeleteAsync(Restaurant entity);
        Task UpdateAsync(Restaurant entity);
        Task FavoriteAsync(FavoriteRestaurant entity);
        Task UnFavoriteAsync(FavoriteRestaurant entity);
        Task FavoriteDishAsync(FavoriteDish entity);
        Task UnFavoriteDishAsync(FavoriteDish entity);
        Task<FavoriteRestaurant?> GetFavoriteRestaurant(string userId, int restaurantId);
        Task<FavoriteDish?> GetFavoriteDish(string userId, int restaurantId, int dishId);
    }
}
