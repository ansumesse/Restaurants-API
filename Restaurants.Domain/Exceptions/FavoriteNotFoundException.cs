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
