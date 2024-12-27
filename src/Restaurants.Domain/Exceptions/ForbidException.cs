namespace Restaurants.Domain.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException(string message = "Access forbidden")
            : base(message)
        {
        }
    }

}
