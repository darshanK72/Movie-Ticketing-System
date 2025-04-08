using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Queries.Payments;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Queries.Payments
{
    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentMethodInfo>>
    {
        public Task<List<PaymentMethodInfo>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            var paymentMethods = new List<PaymentMethodInfo>
            {
                new PaymentMethodInfo
                {
                    Method = PaymentMethod.CreditCard,
                    Name = "Credit Card",
                    Description = "Pay using your credit card",
                    RequiredFields = new List<string> { "CardNumber", "CardHolderName", "ExpiryDate", "CVV" }
                },
                new PaymentMethodInfo
                {
                    Method = PaymentMethod.DebitCard,
                    Name = "Debit Card",
                    Description = "Pay using your debit card",
                    RequiredFields = new List<string> { "CardNumber", "CardHolderName", "ExpiryDate", "CVV" }
                },
                new PaymentMethodInfo
                {
                    Method = PaymentMethod.UPI,
                    Name = "UPI",
                    Description = "Pay using UPI (Unified Payments Interface)",
                    RequiredFields = new List<string> { "UPIId" }
                },
                new PaymentMethodInfo
                {
                    Method = PaymentMethod.NetBanking,
                    Name = "Net Banking",
                    Description = "Pay using your bank account",
                    RequiredFields = new List<string> { "BankName", "AccountNumber" }
                },
                new PaymentMethodInfo
                {
                    Method = PaymentMethod.Wallet,
                    Name = "Digital Wallet",
                    Description = "Pay using digital wallet",
                    RequiredFields = new List<string> { "WalletType", "WalletId" }
                }
            };

            return Task.FromResult(paymentMethods);
        }
    }
} 