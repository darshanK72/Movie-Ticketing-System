using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class DownloadBookingTicketCommand : IRequest<byte[]>
    {
        public string Id { get; set; }

        public DownloadBookingTicketCommand(string id)
        {
            Id = id;
        }
    }
}
