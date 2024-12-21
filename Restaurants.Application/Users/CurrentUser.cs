namespace Restaurants.Application.Users
{
    public record CurrentUser(string Id, string Email, IEnumerable<string> Roles, string? Nationality, DateOnly? DateOfBirth)
    {
        public bool IsInRol(string role) => Roles.Contains(role); 
    }
}
