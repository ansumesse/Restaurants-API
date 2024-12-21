using FluentValidation;
using Restaurants.Application.Restuarants.Commands.UpdateRestaurant;

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
