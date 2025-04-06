using AutoMapper;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Application.Commands.Theaters;

namespace MovieTicketingSystem.Application.Mappings
{
    public class TheaterProfile : Profile
    {
        public TheaterProfile()
        {
            CreateMap<CreateTheaterCommand, Theater>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.CinemaHalls, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateTheaterCommand, Theater>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.CinemaHalls, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateTheaterCommand, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.AddressDetails))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateTheaterCommand, Address>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId))
              .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.AddressDetails))
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
              .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
              .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

              CreateMap<CreateCinemaHallCommand, CinemaHall>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TheaterId, opt => opt.MapFrom(src => Guid.Parse(src.TheaterId!)))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Seats, opt => opt.Ignore())
                .ForMember(dest => dest.Shows, opt => opt.Ignore());

            CreateMap<UpdateCinemaHallCommand, CinemaHall>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id!)))
                .ForMember(dest => dest.TheaterId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Seats, opt => opt.Ignore())
                .ForMember(dest => dest.Shows, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CinemaHall, CinemaHallDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.TheaterId, opt => opt.MapFrom(src => src.TheaterId.ToString()))
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seats));

            CreateMap<Seat, SeatDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.CinemaHallId, opt => opt.MapFrom(src => src.CinemaHallId.ToString()));

            CreateMap<Theater, TheaterDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<Address, AddressDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
} 