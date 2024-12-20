using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.UpdateDish
{
    public class UpdateDishCommand : IRequest
    {
        public int RestaurantId { get; set; } 
        public int DishId { get; set; }
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int? KiloCalories { get; set; }
    }
}
