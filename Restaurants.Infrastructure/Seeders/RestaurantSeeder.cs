using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    dbContext.Restaurants.AddRange(restaurants);
                    await dbContext.SaveChangesAsync();
                }
            }

        }

        private IEnumerable<Restaurant> GetRestaurants() 
        {
            List<Restaurant> restaurants = [
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "",
                    ContactEmail = "hello@kfc.com",
                    HasDelivery = true,
                    Dishes = [
                        new Dish()
                        {
                            Name = "Nashville Hot Chicken",
                            Description = "",
                            Price = 10.3M,
                        },
                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Description = "",
                            Price = 5.3M,
                        }
                    ],
                    Address = new Address()
                    {
                        City = "London",
                        Street = "5 Cork Street",
                        PostalCode = "W1F 8SR",

                    }

                },
                new Restaurant()
                {
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description = "",
                    ContactEmail = "hello@maccas.com.au",
                    HasDelivery = true,

                    Address = new Address()
                    {
                        City = "London",
                        Street = "Boots 193",
                        PostalCode = "W1F 8SR",

                    }
                }
            ];

            return restaurants;

        }
    }
}
