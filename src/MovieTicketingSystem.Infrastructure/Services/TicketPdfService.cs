using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MovieTicketingSystem.Infrastructure.Services
{
    public class TicketPdfService : ITicketPdfService
    {
        public byte[] GenerateTicketPdf(TicketDTO ticket)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Movie Ticket")
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium)
                        .AlignCenter();

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(7);
                                });

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Movie").Bold();
                                table.Cell().Padding(5).Text(ticket.MovieTitle);

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Theater").Bold();
                                table.Cell().Padding(5).Text(ticket.TheaterName);

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Cinema Hall").Bold();
                                table.Cell().Padding(5).Text(ticket.CinemaHallName);

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Show Time").Bold();
                                table.Cell().Padding(5).Text(ticket.ShowDateTime.ToString("g"));

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Seats").Bold();
                                table.Cell().Padding(5).Text(string.Join(", ", ticket.SeatNumbers));

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Number of Tickets").Bold();
                                table.Cell().Padding(5).Text(ticket.NumberOfTickets.ToString());

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Total Amount").Bold();
                                table.Cell().Padding(5).Text($"${ticket.TotalAmount:F2}");

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Customer Name").Bold();
                                table.Cell().Padding(5).Text(ticket.UserName);

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Customer Email").Bold();
                                table.Cell().Padding(5).Text(ticket.UserEmail);

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Booking Status").Bold();
                                table.Cell().Padding(5).Text(ticket.BookingStatus ?? "Unknown");

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Payment Status").Bold();
                                table.Cell().Padding(5).Text(ticket.PaymentStatus ?? "Unknown");

                                table.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Booking Date").Bold();
                                table.Cell().Padding(5).Text(ticket.BookingDate.ToString("g"));
                            });

                            x.Item().PaddingTop(1, Unit.Centimetre)
                                .Text("Terms and Conditions:")
                                .SemiBold()
                                .FontSize(9);

                            x.Item().Text(text =>
                            {
                                text.Span("1. Please arrive at least 15 minutes before the show time.\n");
                                text.Span("2. This ticket is non-transferable.\n");
                                text.Span("3. No refunds for no-shows.\n");
                                text.Span("4. Please carry a valid ID proof.");
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Thank you for choosing our service!");
                            x.Span($" | Generated on {DateTime.Now:g}");
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
} 