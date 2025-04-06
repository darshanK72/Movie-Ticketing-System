using AutoMapper;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ShowId, opt => opt.MapFrom(src => src.ShowId.ToString()))
                .ForMember(dest => dest.NumberOfTickets, opt => opt.MapFrom(src => src.NumberOfTickets))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.BookingStatus, opt => opt.MapFrom(src => src.BookingStatus.ToString()))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.CancellationDate, opt => opt.MapFrom(src => src.CancellationDate))
                .ForMember(dest => dest.CancellationReason, opt => opt.MapFrom(src => src.CancellationReason))
                .ForMember(dest => dest.ShowSeats, opt => opt.MapFrom(src => src.ShowSeats))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));

            CreateMap<Payment, PaymentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId.ToString()))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.RefundDate, opt => opt.MapFrom(src => src.RefundDate))
                .ForMember(dest => dest.RefundReason, opt => opt.MapFrom(src => src.RefundReason))
                .ForMember(dest => dest.FailureReason, opt => opt.MapFrom(src => src.FailureReason));

            CreateMap<Seat, SeatDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.SeatNumber))
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
                .ForMember(dest => dest.ColumnNumber, opt => opt.MapFrom(src => src.ColumnNumber))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.PriceMultiplier, opt => opt.MapFrom(src => src.PriceMultiplier))
                .ForMember(dest => dest.CinemaHallId, opt => opt.MapFrom(src => src.CinemaHallId.ToString()));

            CreateMap<ShowSeat, ShowSeatDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ShowId, opt => opt.MapFrom(src => src.ShowId.ToString()))
                .ForMember(dest => dest.SeatId, opt => opt.MapFrom(src => src.SeatId.ToString()))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seat!.SeatNumber))
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.Seat!.RowNumber))
                .ForMember(dest => dest.ColumnNumber, opt => opt.MapFrom(src => src.Seat!.ColumnNumber))
                .ForMember(dest => dest.SeatType, opt => opt.MapFrom(src => src.Seat!.Type.ToString()))
                .ForMember(dest => dest.PriceMultiplier, opt => opt.MapFrom(src => src.Seat!.PriceMultiplier))
                .ForMember(dest => dest.IsBooked, opt => opt.MapFrom(src => src.IsBooked))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId.ToString()))
                .ForMember(dest => dest.BookingStatus, opt => opt.MapFrom(src => src.BookingStatus.ToString()));
        }
    }
} 