using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.UpdateDish
{
    public class UpdateDishCommandValidator : AbstractValidator<UpdateDishCommand>
    {
        public UpdateDishCommandValidator()
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
