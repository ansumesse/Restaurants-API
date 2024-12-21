using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restuarants.Commands.FavoriteRestaurant;
using Restaurants.Application.Restuarants.Commands.UnFavoriteRestaurant;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]/Restaurant/{id}")]
    [ApiController]
    public class FavoritesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> FavoriteRestaurant(int id)
        {
            await mediator.Send(new FavoriteRestaurantCommand(id));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> UnFavoriteRestaurant(int id)
        {
            await mediator.Send(new UnFavoriteRestaurantCommand(id));
            return NoContent();
        }


    }
}
