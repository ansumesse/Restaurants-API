using FluentValidation;
using Restaurants.Application.Restuarants.Queries.GetAllRestaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Validators
{
    public class GetAllRestaurantsValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private readonly int[] allowedPageSizes = { 5, 10, 15, 30 };
        public GetAllRestaurantsValidator()
        {
            RuleFor(r => r.PageSize)
                .Must(p => allowedPageSizes.Contains(p))
                .WithMessage($"Page size must be in [{string.Join(",", allowedPageSizes)}]");

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);
        }
    }
}
