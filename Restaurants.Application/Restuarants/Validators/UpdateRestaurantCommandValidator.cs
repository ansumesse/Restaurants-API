using FluentValidation;
using Restaurants.Application.Restuarants.Commands.UpdateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Validators
{
    public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
    {
        public UpdateRestaurantCommandValidator()
        {
            RuleFor(r => r.Name)
               .Length(3, 100);

            RuleFor(r => r.Description)
                .NotEmpty();
        }
    }
}
