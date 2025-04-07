using MediatR;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetShowSeatsByShowTimingIdQuery : IRequest<IEnumerable<MovieTicketingSystem.Domain.DTOs.ShowSeatDTO>>
    {
        public Guid ShowTimingId { get; set; }

        public GetShowSeatsByShowTimingIdQuery(Guid showTimingId)
        {
            ShowTimingId = showTimingId;
        }
    }
} 