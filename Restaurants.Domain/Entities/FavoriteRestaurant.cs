namespace Restaurants.Domain.Entities
{
    public class FavoriteRestaurant
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime FavoritedAt { get; set; }
    }
}
