using MediatR;

namespace Restaurants.Application.Users.Commands.UnassignUserRole
{
    public class UnassignUserRoleCommand : IRequest
    {
        public string Role { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
