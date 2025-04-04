using MovieTicketingSystem.Extensions;
using MovieTicketingSystem.Middlewares;
using MovieTicketingSystem.Application.Extensions;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Infrastructure.Extensions;
using MovieTicketingSystem.Domain.Contracts.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ISeederService>();

await seeder.Seed();

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
