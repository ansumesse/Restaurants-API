﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.DeleteDish
{
    public class DeleteDishCommand(int restuarantId, int dishId) : IRequest
    {
        public int RestaurantId { get; } = restuarantId;
        public int DishId { get; } = dishId;
    }
}
