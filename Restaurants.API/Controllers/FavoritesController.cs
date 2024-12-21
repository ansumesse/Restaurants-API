using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restuarants.Commands.FavoriteDish;
using Restaurants.Application.Restuarants.Commands.FavoriteRestaurant;
using Restaurants.Application.Restuarants.Commands.UnFavoriteDish;
using Restaurants.Application.Restuarants.Commands.UnFavoriteRestaurant;
using Restaurants.Application.Restuarants.Queries.GetFavoriteRestaurants;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController(IMediator mediator) : ControllerBase
    {
        [HttpGet("Restuarant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetFavoriteRestaurants()
        {
            var restaurants = await mediator.Send(new GetFavoriteRestaurantsQuery());
            return Ok(restaurants);
        }

        [HttpPost("Restaurant/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FavoriteRestaurant(int id)
        {
            await mediator.Send(new FavoriteRestaurantCommand(id));
            return NoContent();
        }

        [HttpDelete("Restaurant/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnFavoriteRestaurant(int id)
        {
            await mediator.Send(new UnFavoriteRestaurantCommand(id));
            return NoContent();
        }

        [HttpPost("Restaurant/{id}/Dish/{dishId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FavoriteDish(int id, int dishId)
        {
            await mediator.Send(new FavoriteDishCommand(id, dishId));
            return NoContent();
        }

        [HttpDelete("Restaurant/{id}/Dish/{dishId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnFavoriteDish(int id, int dishId)
        {
            await mediator.Send(new UnFavoriteDishCommand(id, dishId));
            return NoContent();
        }


    }
}
