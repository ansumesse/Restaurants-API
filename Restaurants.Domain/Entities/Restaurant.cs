﻿namespace Restaurants.Domain.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }

        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }

        public Address? Address { get; set; }
        public ICollection<Dish> Dishes { get; set; } = new HashSet<Dish>();

        public User Owner { get; set; } = default!;

        public string OwnerId { get; set; } = default!;
    }
}
