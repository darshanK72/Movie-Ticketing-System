using AutoMapper;
using MovieTicketingSystem.Application.Commands.Shows;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Mappings
{
    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
            CreateMap<Show, ShowDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId.ToString()))
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie!.Title))
                .ForMember(dest => dest.CinemaHallId, opt => opt.MapFrom(src => src.CinemaHallId.ToString()))
                .ForMember(dest => dest.CinemaHallName, opt => opt.MapFrom(src => src.CinemaHall!.Name))
                .ForMember(dest => dest.TheaterId, opt => opt.MapFrom(src => src.CinemaHall!.TheaterId.ToString()))
                .ForMember(dest => dest.TheaterName, opt => opt.MapFrom(src => src.CinemaHall!.Theater!.Name))
                .ForMember(dest => dest.ShowManagerId, opt => opt.MapFrom(src => src.ShowManagerId))
                .ForMember(dest => dest.ShowManagerName, opt => opt.MapFrom(src => src.ShowManager != null ? $"{src.ShowManager.FirstName} {src.ShowManager.LastName}" : null))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.TotalSeats, opt => opt.MapFrom(src => src.TotalSeats))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());
               
            CreateMap<ShowDTO, Show>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id!)))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => Guid.Parse(src.MovieId!)))
                .ForMember(dest => dest.CinemaHallId, opt => opt.MapFrom(src => Guid.Parse(src.CinemaHallId!)))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime!.Value))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime!.Value))
                .ForMember(dest => dest.TotalSeats, opt => opt.MapFrom(src => src.TotalSeats))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ShowManagerId, opt => opt.MapFrom(src => src.ShowManagerId))
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.CinemaHall, opt => opt.Ignore())
                .ForMember(dest => dest.ShowManager, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.ShowSeats, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateShowCommand, Show>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.CinemaHallId, opt => opt.MapFrom(src => src.CinemaHallId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ShowManagerId, opt => opt.MapFrom(src => src.ShowManagerId.HasValue ? src.ShowManagerId.Value.ToString() : null))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.CinemaHall, opt => opt.Ignore())
                .ForMember(dest => dest.ShowManager, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.ShowSeats, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.TotalSeats, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableSeats, opt => opt.Ignore());
        }
    }
} 