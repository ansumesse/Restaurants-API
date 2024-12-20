using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Common;
using Restaurants.Application.Restuarants.Commands.CreateRestaurant;
using Restaurants.Application.Restuarants.Commands.DeleteRestaurant;
using Restaurants.Application.Restuarants.Commands.FavoriteRestaurant;
using Restaurants.Application.Restuarants.Commands.UpdateRestaurant;
using Restaurants.Application.Restuarants.Dtos;
using Restaurants.Application.Restuarants.Queries.GetAllRestaurants;
using Restaurants.Application.Restuarants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization.Constants;
namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = PolicyNames.AtLeast2Restaurants)]
        public async Task<ActionResult<PagedResult<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> Create(CreateRestaurantCommand command)
        {
            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
                return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
                return NoContent();
        }

        [HttpPost("FavoriteRestaurant/{id}")]
        public async Task<IActionResult> FavoriteRestaurant(int id)
        {
            await mediator.Send(new FavoriteRestaurantCommand(id));
            return NoContent();
        }
    }
}
