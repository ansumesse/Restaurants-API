using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            if(await context.Database.CanConnectAsync())
            {
                //if(!await context.Restaurants.AnyAsync())
                //{
                //    var restuarants = GetRestaurants();
                //    await context.AddRangeAsync(restuarants);
                //    await context.SaveChangesAsync();
                //}

                if(!await context.Roles.AnyAsync())
                {
                    await context.AddRangeAsync(GetRoles());
                    await context.SaveChangesAsync();
                    await AdminSeeder();
                }
            }
        }

        private async Task AdminSeeder()
        {
            var result = await userManager.CreateAsync(new User
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                DateOfBirth = new DateOnly(2003, 5, 1),
                Nationality = "Egyptian"
            }, "Admin123*");
            
                var user = await userManager.FindByEmailAsync("admin@gmail.com");
                var role = await roleManager.FindByNameAsync(UserRoles.Admin);
                await userManager.AddToRoleAsync(user!, role!.Name!);

            
        }
        private IEnumerable<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles = [
                new(UserRoles.User) 
                {
                    NormalizedName = UserRoles.User.ToUpper()
                },
                new(UserRoles.Owner)
                {
                    NormalizedName = UserRoles.Owner.ToUpper()
                },
                new(UserRoles.Admin)
                {
                    NormalizedName = UserRoles.Admin.ToUpper()
                }
                ];
            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = [
                new()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes =
                [
                    new ()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30M,
                    },

                    new ()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30M,
                    },
                ],
                Address = new ()
                {
                    City = "London",
                    Street = "Cork St 5",
                    PostalCode = "WC2N 5DU"
                }
            },
                new ()
                {
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description =
                        "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Address = new Address()
                    {
                        City = "London",
                        Street = "Boots 193",
                        PostalCode = "W1F 8SR"
                    }
                }
                    ];
            return restaurants;
        }
    }
}
