using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Domain.Contracts.Services
{
    public interface ITicketPdfService
    {
        byte[] GenerateTicketPdf(TicketDTO ticket);
    }
}
