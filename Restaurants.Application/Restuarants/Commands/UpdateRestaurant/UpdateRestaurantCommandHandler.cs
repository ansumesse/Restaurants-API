using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restuarants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper) : IRequestHandler<UpdateRestaurantCommand>
    {
        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Update Restaurant #{RestaurantId} with {@Restaurant}", request.Id, request);
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            await restaurantsRepository.UpdateAsync(mapper.Map(request, restaurant));
        }
    }
}
