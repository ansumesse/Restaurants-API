using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPatch("User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost("UserRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        
        [HttpDelete("UserRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnassignUserRole(UnassignUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
