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
