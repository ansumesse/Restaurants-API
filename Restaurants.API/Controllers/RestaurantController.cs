using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restuarants.Commands.CreateRestaurant;
using Restaurants.Application.Restuarants.Commands.DeleteRestaurant;
using Restaurants.Application.Restuarants.Commands.UpdateRestaurant;
using Restaurants.Application.Restuarants.Dtos;
using Restaurants.Application.Restuarants.Queries.GetAllRestaurants;
using Restaurants.Application.Restuarants.Queries.GetRestaurantById;
namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            return Ok(await mediator.Send(new GetAllRestaurantsQuery()));
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
    }
}
