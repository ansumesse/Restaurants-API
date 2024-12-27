using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignUserRole
{
    public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassignUserRoleCommand>
    {
        public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("UnAssigning user role: {@Request}", request);

            var user = await userManager.FindByEmailAsync(request.Email)
               ?? throw new NotFoundException(nameof(User), request.Email);

            var role = await roleManager.FindByNameAsync(request.Role)
                ?? throw new NotFoundException(nameof(IdentityRole), request.Role);

            await userManager.RemoveFromRoleAsync(user, role.Name!);
        }
    }
}
