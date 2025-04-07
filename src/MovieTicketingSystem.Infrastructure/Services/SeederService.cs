using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Persistence;
namespace MovieTicketingSystem.Infrastructure.Services;

internal class SeederService : ISeederService
{
    private readonly TicketingDbContext _dbContext;

    public SeederService(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Seed()
    {
        if (_dbContext.Database.GetPendingMigrations().Any())
        {
            await _dbContext.Database.MigrateAsync();
        }

        if (await _dbContext.Database.CanConnectAsync())
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Genres.Any())
            {
                var genres = GetGenres();
                _dbContext.Genres.AddRange(genres);
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Languages.Any())
            {
                var languages = GetLanguages();
                _dbContext.Languages.AddRange(languages);
                await _dbContext.SaveChangesAsync();
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
                }, 
                new (UserRole.Admin.ToString())
                {
                    NormalizedName = UserRole.Admin.ToString().ToUpper()
                },
                new (UserRole.TheaterManager.ToString())
                {
                    NormalizedName = UserRole.TheaterManager.ToString().ToUpper()
                }
            ];

        return roles;
    }

    private IEnumerable<Genre> GetGenres()
    {
        var genres = new List<Genre>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Action",
                Description = "Action-packed movies with intense sequences",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Comedy",
                Description = "Humorous and entertaining movies",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Drama",
                Description = "Character-driven stories with emotional depth",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Horror",
                Description = "Scary and suspenseful movies",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Romance",
                Description = "Love stories and romantic relationships",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sci-Fi",
                Description = "Science fiction and futuristic stories",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Thriller",
                Description = "Suspenseful and tension-filled movies",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        return genres;
    }

    private IEnumerable<Language> GetLanguages()
    {
        var languages = new List<Language>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Hindi",
                Code = "hi",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "English",
                Code = "en",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Bengali",
                Code = "bn",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Telugu",
                Code = "te",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Tamil",
                Code = "ta",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Kannada",
                Code = "kn",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Malayalam",
                Code = "ml",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Marathi",
                Code = "mr",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Gujarati",
                Code = "gu",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Punjabi",
                Code = "pa",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        return languages;
    }
}
