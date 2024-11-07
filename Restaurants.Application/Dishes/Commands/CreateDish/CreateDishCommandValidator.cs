using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
    {
        public CreateDishCommandValidator()
        {
            RuleFor(d => d.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price Cannot be Negative value");

            RuleFor(d => d.KiloCalories)
                .GreaterThanOrEqualTo(0)
                .WithMessage("KiloCalories Cannot be Negative value");
        }
    }
}
