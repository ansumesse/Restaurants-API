using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("User context is not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
            var dOB = DateOnly.ParseExact(user.FindFirst(c => c.Type == AppClaimTypes.DateOfBirth)?.Value!, "yyyy-MM-dd");
            var nationality = user.FindFirst(c => c.Type == AppClaimTypes.Nationality)?.Value;

            return new CurrentUser(userId, email, roles, nationality, dOB);
        }
    }
}
