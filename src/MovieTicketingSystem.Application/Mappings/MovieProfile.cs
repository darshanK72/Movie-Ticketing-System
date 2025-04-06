using AutoMapper;
using MovieTicketingSystem.Application.Commands.Movies;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Mappings
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres!.Select(g => g.Name)))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages!.Select(l => l.Name)));

            CreateMap<MovieDTO, Movie>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id!)))
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Languages, opt => opt.Ignore())
                .ForMember(dest => dest.Shows, opt => opt.Ignore());
            
            CreateMap<CreateMovieCommand, Movie>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Languages, opt => opt.Ignore())
                .ForMember(dest => dest.Shows, opt => opt.Ignore());

            CreateMap<UpdateMovieCommand, Movie>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id!)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Languages, opt => opt.Ignore())
                .ForMember(dest => dest.Shows, opt => opt.Ignore());
        }
    }
} 