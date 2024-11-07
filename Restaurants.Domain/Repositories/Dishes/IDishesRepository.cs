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
        Task CreateAsync(Dish entity);
    }
}
