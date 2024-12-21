using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task DeleteAsync(Restaurant entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task FavoriteAsync(FavoriteRestaurant entity)
        {
            await context.FavoriteRestaurants.AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public async Task UnFavoriteAsync(FavoriteRestaurant entity)
        {
            context.FavoriteRestaurants.Remove(entity);
            await context.SaveChangesAsync();
        }
        public async Task FavoriteDishAsync(FavoriteDish entity)
        {
            await context.FavoriteDishes.AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public async Task UnFavoriteDishAsync(FavoriteDish entity)
        {
            context.FavoriteDishes.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<FavoriteRestaurant?> GetFavoriteRestaurant(string userId, int restaurantId)
        {
            return await context.FavoriteRestaurants.FirstOrDefaultAsync(f => f.UserId == userId && f.RestaurantId == restaurantId);
        }
        
        public async Task<FavoriteDish?> GetFavoriteDish(string userId, int restaurantId, int dishId)
        {
            return await context.FavoriteDishes.FirstOrDefaultAsync(f => f.UserId == userId && f.RestaurantId == restaurantId && f.DishId == dishId);
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await context.Restaurants.ToListAsync();
        }

        public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchedPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
        {
            var searchedPhraseLower = searchedPhrase?.ToLower();
            var baseQuery = context.Restaurants
                .Where(r => searchedPhraseLower == null || (EF.Functions.Like(r.Name, $"%{searchedPhraseLower}%") ||
                EF.Functions.Like(r.Description, $"%{searchedPhraseLower}%")));

            var restaurantsCount = await baseQuery.CountAsync();

            if(sortBy != null)
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name), r => r.Name },
                    {nameof(Restaurant.Description), r => r.Description },
                    {nameof(Restaurant.Category), r => r.Category }
                };

                var selectedColumn = columnSelectors[sortBy];

                baseQuery = sortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn) 
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var result = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (result, restaurantsCount);
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            return await context.Restaurants.Include(r => r.Dishes).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Restaurant entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }


    }
}
