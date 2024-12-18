using FluentValidation;
using Restaurants.Application.Restuarants.Queries.GetAllRestaurants;
using Restaurants.Domain.Entities;
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
        private readonly string[] allowedSortByColumnNames =
        {
            nameof(Restaurant.Name),
            nameof(Restaurant.Description),
            nameof(Restaurant.Category)
        };
        public GetAllRestaurantsValidator()
        {
            RuleFor(r => r.PageSize)
                .Must(p => allowedPageSizes.Contains(p))
                .WithMessage($"Page size must be in [{string.Join(",", allowedPageSizes)}]");

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(q => q.SortBy)
                .Must(p => allowedSortByColumnNames.Contains(p))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
