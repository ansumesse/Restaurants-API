﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restuarants.Dtos;
using Restaurants.Domain.Repositories.Restaurants;

namespace Restaurants.Application.Restuarants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
    {
        public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all restaurants");
            var (restaurants, restaurantsCount) = await restaurantsRepository.GetAllMatchingAsync(request.SearchedPhrase, request.PageSize, request.PageNumber, request.SortBy, request.SortDirection);

            var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return new PagedResult<RestaurantDto>(restaurantsDto, restaurantsCount, request.PageSize, request.PageNumber);
        }
    }
}
