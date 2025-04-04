using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Infrastructure.Persistence;
using MovieTicketingSystem.Infrastructure.Services;

namespace MovieTicketingSystem.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<TicketingDbContext>(options => 
            options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>()
        .AddEntityFrameworkStores<TicketingDbContext>()
        .AddDefaultTokenProviders();

        // Add a dummy email sender for development
        //services.AddScoped<IEmailSender<User>, DummyEmailSender>();

        services.AddScoped<IMovieTicketingSystemSeeder, MovieTicketingSystemSeeder>();
        //services.AddScoped<IMovieTicketingSystemRepository, MovieTicketingSystemRepository>();
        //services.AddScoped<IDishesRepository, DishesRepository>();
        //services.AddAuthorizationBuilder()
        //    .AddPolicy(PolicyNames.HasNationality, 
        //        builder => builder.RequireClaim(AppClaimTypes.Nationality, "German", "Polish"))
        //    .AddPolicy(PolicyNames.AtLeast20,
        //        builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
        //    .AddPolicy(PolicyNames.CreatedAtleast2MovieTicketingSystem, 
        //        builder => builder.AddRequirements(new CreatedMultipleMovieTicketingSystemRequirement(2)));

        //services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        //services.AddScoped<IAuthorizationHandler, CreatedMultipleMovieTicketingSystemRequirementHandler>();
        //services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();

        //services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
        //services.AddScoped<IBlobStorageService, BlobStorageService>();
    }
}
