using MediatR;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetShowTimingsByShowIdQuery : IRequest<IEnumerable<MovieTicketingSystem.Domain.DTOs.ShowTimingDTO>>
    {
        public Guid ShowId { get; set; }

        public GetShowTimingsByShowIdQuery(Guid showId)
        {
            ShowId = showId;
        }
    }
} 