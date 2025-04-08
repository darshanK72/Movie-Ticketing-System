using MediatR;
using System.Collections.Generic;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Queries.Payments
{
    public class GetPaymentMethodsQuery : IRequest<List<PaymentMethodInfo>>
    {
    }

    public class PaymentMethodInfo
    {
        public PaymentMethod Method { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? RequiredFields { get; set; } = new List<string>();
    }
} 