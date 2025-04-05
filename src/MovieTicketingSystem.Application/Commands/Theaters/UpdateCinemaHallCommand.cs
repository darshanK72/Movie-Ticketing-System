using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Theaters
{
    public class UpdateCinemaHallCommand : IRequest<bool>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? TotalSeats { get; set; }
        public int? NumberOfRows { get; set; }
        public int? SeatsPerRow { get; set; }
        public bool? Has3D { get; set; }
        public bool? HasDolby { get; set; }
    }
}
