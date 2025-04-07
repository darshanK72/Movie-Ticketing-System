using AutoMapper;
using MovieTicketingSystem.Application.Commands.Shows;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

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
                .ForMember(dest => dest.TheaterName, opt => opt.MapFrom(src => src.CinemaHall!.Theater!.Name));

            CreateMap<ShowDTO, Show>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id!)))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => Guid.Parse(src.MovieId!)))
                .ForMember(dest => dest.CinemaHallId, opt => opt.MapFrom(src => Guid.Parse(src.CinemaHallId!)))
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.CinemaHall, opt => opt.Ignore())
                .ForMember(dest => dest.ShowTimings, opt => opt.Ignore());

            CreateMap<ShowTiming, ShowTimingDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ShowId, opt => opt.MapFrom(src => src.ShowId.ToString()))
                .ForMember(dest => dest.ShowManagerId, opt => opt.MapFrom(src => src.ShowManagerId))
                .ForMember(dest => dest.ShowManagerName, opt => opt.MapFrom(src => src.ShowManager != null ? $"{src.ShowManager.FirstName} {src.ShowManager.LastName}" : null))
                .ForMember(dest => dest.ShowStatus, opt => opt.MapFrom(src => src.ShowStatus.ToString()));

            CreateMap<ShowTimingDTO, ShowTiming>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id!)))
                .ForMember(dest => dest.ShowId, opt => opt.MapFrom(src => Guid.Parse(src.ShowId!)))
                .ForMember(dest => dest.ShowManagerId, opt => opt.MapFrom(src => src.ShowManagerId))
                .ForMember(dest => dest.ShowStatus, opt => opt.MapFrom(src => Enum.Parse<ShowStatus>(src.ShowStatus!)))
                .ForMember(dest => dest.Show, opt => opt.Ignore())
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
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.CinemaHall, opt => opt.Ignore())
                .ForMember(dest => dest.ShowTimings, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        }
    }
} 