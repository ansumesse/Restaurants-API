﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence
{
    internal class RestaurantDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public DbSet<FavoriteDish> FavoriteDishes { get; set; }
        internal DbSet<FavoriteRestaurant> FavoriteRestaurants { get; set; }
        internal DbSet<Restaurant> Restaurants  { get; set; }
        internal DbSet<Dish> Dishes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>()
                .OwnsOne<Address>(r => r.Address);

            modelBuilder.Entity<User>()
                .HasMany(o => o.OwnedRestaurants)
                .WithOne(r => r.Owner)
                .HasForeignKey(r => r.OwnerId);
        }
    }
}
