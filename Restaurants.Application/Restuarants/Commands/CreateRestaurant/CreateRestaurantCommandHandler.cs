using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository,
        ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper) : IRequestHandler<CreateRestaurantCommand, int>
    {
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating restaurant");
            return await restaurantsRepository.CreateAsync(mapper.Map<Restaurant>(request));
        }
    }
}
