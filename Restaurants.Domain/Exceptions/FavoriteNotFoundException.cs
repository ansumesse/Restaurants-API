using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Exceptions
{
    public class FavoriteNotFoundException : Exception
    {
        public FavoriteNotFoundException(string resourceType, string resourceId)
            : base($"The favorite {resourceType} with id: {resourceId} was not found.")
        {
        }
    }
}
