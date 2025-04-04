using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Persistence;
namespace MovieTicketingSystem.Infrastructure.Services;

internal class SeederService(TicketingDbContext dbContext) : ISeederService
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            //if (!dbContext.MovieTicketingSystem.Any())
            //{
            //    var MovieTicketingSystem = GetMovieTicketingSystem();
            //    dbContext.MovieTicketingSystem.AddRange(MovieTicketingSystem);
            //    await dbContext.SaveChangesAsync();
            //}

            if(!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
            [
                new (UserRole.User.ToString())
                {
                    NormalizedName = UserRole.User.ToString().ToUpper()
                },
                new (UserRole.ShowManager.ToString())
                {
                    NormalizedName = UserRole.ShowManager.ToString().ToUpper()
                }
            ];

        return roles;
    }

    //private IEnumerable<Restaurant> GetMovieTicketingSystem()
    //{
    //    User owner = new User()
    //    {
    //        Email = "seed-user@test.com"
    //    };

    //    List<Restaurant> MovieTicketingSystem = [
    //        new()
    //        {
    //            Owner = owner,
    //            Name = "KFC",
    //            Category = "Fast Food",
    //            Description =
    //                "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
    //            ContactEmail = "contact@kfc.com",
    //            HasDelivery = true,
    //            Dishes =
    //            [
    //                new ()
    //                {
    //                    Name = "Nashville Hot Chicken",
    //                    Description = "Nashville Hot Chicken (10 pcs.)",
    //                    Price = 10.30M,
    //                },

    //                new ()
    //                {
    //                    Name = "Chicken Nuggets",
    //                    Description = "Chicken Nuggets (5 pcs.)",
    //                    Price = 5.30M,
    //                },
    //            ],
    //            Address = new ()
    //            {
    //                City = "London",
    //                Street = "Cork St 5",
    //                PostalCode = "WC2N 5DU"
    //            },
                
    //        },
    //        new ()
    //        {
    //            Owner = owner,
    //            Name = "McDonald",
    //            Category = "Fast Food",
    //            Description =
    //                "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's MovieTicketingSystem.",
    //            ContactEmail = "contact@mcdonald.com",
    //            HasDelivery = true,
    //            Address = new Address()
    //            {
    //                City = "London",
    //                Street = "Boots 193",
    //                PostalCode = "W1F 8SR"
    //            }
    //        }
    //    ];

    //    return MovieTicketingSystem;
    //}
}
