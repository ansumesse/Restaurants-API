using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Services.Restuarants
{
    public interface IRestaurantsService
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
    }
}
