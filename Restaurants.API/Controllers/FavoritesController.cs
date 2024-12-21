using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class FavoritesController(IMediator mediator) : ControllerBase
    {
        [HttpGet("Restuarant")]
        public async Task<IActionResult> GetFavoriteRestaurants()
        {
            var restaurants = await mediator.Send(new GetFavoriteRestaurantsQuery());
            return Ok(restaurants);
        }

        [HttpPost("Restaurant/{id}")]
        public async Task<IActionResult> FavoriteRestaurant(int id)
        {
            await mediator.Send(new FavoriteRestaurantCommand(id));
            return NoContent();
        }

        [HttpDelete("Restaurant/{id}")]
        public async Task<IActionResult> UnFavoriteRestaurant(int id)
        {
            await mediator.Send(new UnFavoriteRestaurantCommand(id));
            return NoContent();
        }

        [HttpPost("Restaurant/{id}/Dish/{dishId}")]
        public async Task<IActionResult> FavoriteDish(int id, int dishId)
        {
            await mediator.Send(new FavoriteDishCommand(id, dishId));
            return NoContent();
        }

        [HttpDelete("Restaurant/{id}/Dish/{dishId}")]
        public async Task<IActionResult> UnFavoriteDish(int id, int dishId)
        {
            await mediator.Send(new UnFavoriteDishCommand(id, dishId));
            return NoContent();
        }


    }
}
