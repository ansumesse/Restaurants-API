using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Services
{
    public interface IRestaurantsService
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant> GetRestaurantByIdAsync(int id);
    }
}
