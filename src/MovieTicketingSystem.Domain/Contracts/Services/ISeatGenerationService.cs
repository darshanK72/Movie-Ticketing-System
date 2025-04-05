using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Domain.Contracts.Services
{
    public interface ISeatGenerationService
    {
        List<Seat> GenerateSeatsForCinemaHall(CinemaHall cinemaHall);
    }
}
