using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
        IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var user = userContext.GetCurrentUser();

            logger.LogInformation("User: {Email}, date of birth {DoB} - Handling MinimumAgeRequirement",
           user.Email,
           user.DateOfBirth);

            if (user.DateOfBirth == null)
            {
                logger.LogWarning("User date of birth is null");
                context.Fail();
                return Task.CompletedTask;
            }
            if(DateTime.Today.Year - user.DateOfBirth.Value.Year >= 20)
            {
                logger.LogInformation("Authorization succeeded");
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
