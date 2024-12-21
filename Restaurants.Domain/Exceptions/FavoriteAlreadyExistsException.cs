using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Exceptions
{
        public class FavoriteAlreadyExistsException : Exception
        {
            public FavoriteAlreadyExistsException(string resourceType)
                : base($"You have already favorited this {resourceType}.")
            {
            }
        }
}
