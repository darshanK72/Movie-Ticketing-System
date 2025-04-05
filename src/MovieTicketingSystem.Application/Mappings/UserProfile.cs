using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserCommand, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => MapRole(src.Role)))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.ManagedShows, opt => opt.Ignore());
        }

        private UserRole MapRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                return UserRole.User;

            if (Enum.TryParse<UserRole>(role, out var userRole))
                return userRole;

            return UserRole.User;
        }
    }
}
