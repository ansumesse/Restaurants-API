using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Constants;
using System.Security.Claims;

namespace Restaurants.APITests
{
    internal class FakepolicyEvaluator : IPolicyEvaluator
    {
        public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Email, "Test@test.com"),
                    new Claim(ClaimTypes.Role, UserRoles.Admin),
                    new Claim(ClaimTypes.Role, UserRoles.Owner),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                }, "Test"));
            var ticket = new AuthenticationTicket(claimsPrincipal, "Test");
            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }

        public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object? resource)
        {
            var result = PolicyAuthorizationResult.Success();
            return Task.FromResult(result);
        }
    }
}
