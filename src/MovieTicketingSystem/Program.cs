using MovieTicketingSystem.Extensions;
using MovieTicketingSystem.Middlewares;
using MovieTicketingSystem.Application.Extensions;
using MovieTicketingSystem.Infrastructure.Extensions;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Infrastructure.Persistence;
using MovieTicketingSystem.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TicketingDbContext>();
        context.Database.EnsureCreated();
        
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

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

