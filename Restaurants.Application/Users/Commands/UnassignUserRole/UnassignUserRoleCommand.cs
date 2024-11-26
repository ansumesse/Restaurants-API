using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands.UnassignUserRole
{
    public class UnassignUserRoleCommand : IRequest
    {
        public string Role { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
