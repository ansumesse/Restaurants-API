using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories.Restaurants
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchedPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task<IEnumerable<Restaurant>> GetRestaurantsByIdsAsync(IEnumerable<int> ids);
        Task<int> CreateAsync(Restaurant entity);
        Task DeleteAsync(Restaurant entity);
        Task UpdateAsync(Restaurant entity);
        Task FavoriteAsync(FavoriteRestaurant entity);
        Task UnFavoriteAsync(FavoriteRestaurant entity);
        Task FavoriteDishAsync(FavoriteDish entity);
        Task UnFavoriteDishAsync(FavoriteDish entity);
        Task<IEnumerable<FavoriteRestaurant>> GetFavoriteRestaurants(string userId, int? restaurantId = null);
        Task<FavoriteDish?> GetFavoriteDish(string userId, int restaurantId, int dishId);
    }
}
