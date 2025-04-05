using System;
using System.Collections.Generic;
using System.Linq;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Services
{
    public class SeatGenerationService : ISeatGenerationService
    {
        public List<Seat> GenerateSeatsForCinemaHall(CinemaHall cinemaHall)
        {
            var seats = new List<Seat>();
            int totalSeats = cinemaHall.TotalSeats;
            int rows = cinemaHall.NumberOfRows;
            int seatsPerRow = cinemaHall.SeatsPerRow;

            int premiumSeatsCount = (int)Math.Round(totalSeats * 0.1);
            int reclinerSeatsCount = (int)Math.Round(totalSeats * 0.3);
            int standardSeatsCount = totalSeats - premiumSeatsCount - reclinerSeatsCount;

            int premiumRows = (int)Math.Ceiling(premiumSeatsCount / (double)seatsPerRow);
            int reclinerRows = (int)Math.Ceiling(reclinerSeatsCount / (double)seatsPerRow);
            int standardRows = rows - premiumRows - reclinerRows;

            int seatCounter = 1;
            int rowCounter = 1;

            for (int row = 0; row < standardRows; row++)
            {
                for (int col = 1; col <= seatsPerRow; col++)
                {
                    seats.Add(CreateSeat(seatCounter++, rowCounter, col, SeatType.Standard, cinemaHall.Id));
                }
                rowCounter++;
            }

            for (int row = 0; row < reclinerRows; row++)
            {
                for (int col = 1; col <= seatsPerRow; col++)
                {
                    seats.Add(CreateSeat(seatCounter++, rowCounter, col, SeatType.Recliner, cinemaHall.Id));
                }
                rowCounter++;
            }

            for (int row = 0; row < premiumRows; row++)
            {
                for (int col = 1; col <= seatsPerRow; col++)
                {
                    seats.Add(CreateSeat(seatCounter++, rowCounter, col, SeatType.Premium, cinemaHall.Id));
                }
                rowCounter++;
            }

            return seats;
        }

        private Seat CreateSeat(int seatNumber, int rowNumber, int columnNumber, SeatType type, Guid cinemaHallId)
        {
            decimal priceMultiplier = type switch
            {
                SeatType.Standard => 1.0m,
                SeatType.Recliner => 1.5m,
                SeatType.Premium => 2.0m,
                _ => 1.0m
            };

            return new Seat
            {
                SeatNumber = $"{rowNumber}-{columnNumber}",
                RowNumber = rowNumber,
                ColumnNumber = columnNumber,
                Type = type,
                PriceMultiplier = priceMultiplier,
                CinemaHallId = cinemaHallId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
} 