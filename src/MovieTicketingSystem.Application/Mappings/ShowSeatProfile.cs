using AutoMapper;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Mappings
{
    public class ShowSeatProfile : Profile
    {
        public ShowSeatProfile()
        {
            CreateMap<ShowSeat, ShowSeatDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ShowTimingId, opt => opt.MapFrom(src => src.ShowTimingId.ToString()))
                .ForMember(dest => dest.SeatId, opt => opt.MapFrom(src => src.SeatId.ToString()))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seat.SeatNumber))
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.Seat.RowNumber))
                .ForMember(dest => dest.ColumnNumber, opt => opt.MapFrom(src => src.Seat.ColumnNumber))
                .ForMember(dest => dest.SeatType, opt => opt.MapFrom(src => src.Seat.Type.ToString()))
                .ForMember(dest => dest.PriceMultiplier, opt => opt.MapFrom(src => src.Seat.PriceMultiplier))
                .ForMember(dest => dest.IsBooked, opt => opt.MapFrom(src => src.IsBooked))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId.ToString()))
                .ForMember(dest => dest.SeatBookingStatus, opt => opt.MapFrom(src => src.SeatBookingStatus.ToString()));
        }
    }
} 