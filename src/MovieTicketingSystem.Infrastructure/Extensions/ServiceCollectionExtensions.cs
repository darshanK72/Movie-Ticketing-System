using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieTicketingSystem.Application.Services;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Infrastructure.Persistence;
using MovieTicketingSystem.Infrastructure.Repositories;
using MovieTicketingSystem.Infrastructure.Services;

namespace MovieTicketingSystem.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<TicketingDbContext>(options => 
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            })
            .EnableSensitiveDataLogging());

        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        })
            .AddEntityFrameworkStores<TicketingDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider("Default", typeof(DataProtectorTokenProvider<User>));

        services.Configure<DataProtectionTokenProviderOptions>("Default", options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(1);
        });

        services.AddScoped<ISeederService, SeederService>();
        services.AddSingleton<ISeatGenerationService,SeatGenerationService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITheaterRepository, TheaterRepository>();
    }
}
