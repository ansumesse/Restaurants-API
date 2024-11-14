﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands.AssignUserRole
{
    public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
        ): IRequestHandler<AssignUserRoleCommand>
    {
        public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Assigning user role: {@Request}", request);
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new NotFoundException(nameof(User), request.Email);

            var role = await roleManager.FindByNameAsync(request.Role)
                ?? throw new NotFoundException(nameof(IdentityRole), request.Role);

            await userManager.AddToRoleAsync(user, role.Name!);
        }
    }
}
