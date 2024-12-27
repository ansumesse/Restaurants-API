namespace Restaurants.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string resourceType, string resourceId)
            : base($"{resourceType} with id: {resourceId} does not exist.")
        {
        }
    }
}
