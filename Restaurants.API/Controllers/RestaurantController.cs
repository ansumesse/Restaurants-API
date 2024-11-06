using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restuarants.Commands.CreateRestaurant;
using Restaurants.Application.Restuarants.Commands.DeleteRestaurant;
using Restaurants.Application.Restuarants.Commands.UpdateRestaurant;
using Restaurants.Application.Restuarants.Queries.GetAllRestaurants;
using Restaurants.Application.Restuarants.Queries.GetRestaurantById;
namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await mediator.Send(new GetAllRestaurantsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
            if (restaurant is null)
                return NotFound();
            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantCommand command)
        {
            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            var isUpdated = await mediator.Send(command);
            if (isUpdated)
                return NoContent();
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));
            if (isDeleted)
                return NoContent();
            return NotFound();
        }
    }
}
