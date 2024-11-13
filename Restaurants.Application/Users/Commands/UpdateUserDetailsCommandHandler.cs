using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands
{
    public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger,
        IUserContext userContext,
        IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
    {
        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("Update User Details {@userDetails} with id : {userId}", request, user!.Id);

            var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);
            if (dbUser is null)
                throw new NotFoundException(nameof(User), user!.Id);

            dbUser.DateOfBirth = request.DateOfBirth;
            dbUser.Nationality = request.Nationality;

            await userStore.UpdateAsync(dbUser, cancellationToken);
        }
    }
}
