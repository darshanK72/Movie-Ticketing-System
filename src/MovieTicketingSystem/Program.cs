using MovieTicketingSystem.Extensions;
using MovieTicketingSystem.Middlewares;
using MovieTicketingSystem.Application.Extensions;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Infrastructure.Extensions;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TicketingDbContext>();
        context.Database.EnsureCreated();
        
        // Seed the database
        var seeder = services.GetRequiredService<ISeederService>();
        await seeder.Seed();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

