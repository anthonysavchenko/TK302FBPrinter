using System.Collections.Generic;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Business.Operations.PrintTicket;

namespace TK302FBPrinter.Business.Operations.PrintTicketCinema
{
    public class PrintTicketCinemaOperation : Operation, IPrintTicketCinemaOperation
    {
        private readonly IPrintTicketOperation _printTicketOperation;

        public PrintTicketCinemaOperation(IPrintTicketOperation printTicketOperation)
        {
            _printTicketOperation = printTicketOperation;
        }

        private string CreateKey(string source)
        {
            return "[" + source.ToUpper() + "]";
        }

        public bool Execute(TicketCinema ticketCinema)
        {
            var placeholders = new List<Placeholder>();

            if (!string.IsNullOrEmpty(ticketCinema.Theatre))
            {
                placeholders.Add(new Placeholder
                {
                    Key = CreateKey(nameof(ticketCinema.Theatre)),
                    Value = ticketCinema.Theatre
                });
            }

            if (!string.IsNullOrEmpty(ticketCinema.CompanyName))
            {
                placeholders.Add(new Placeholder()
                {
                    Key = CreateKey(nameof(ticketCinema.CompanyName)),
                    Value = ticketCinema.CompanyName
                });
            }

            if (!string.IsNullOrEmpty(ticketCinema.CompanyAddress))
            {
                placeholders.Add(new Placeholder()
                {
                    Key = CreateKey(nameof(ticketCinema.CompanyAddress)),
                    Value = ticketCinema.CompanyAddress
                });
            }

            if (!string.IsNullOrEmpty(ticketCinema.OGRN))
            {
                placeholders.Add(new Placeholder()
                {
                    Key = CreateKey(nameof(ticketCinema.OGRN)),
                    Value = ticketCinema.OGRN
                });
            }

            if (!string.IsNullOrEmpty(ticketCinema.INN))
            {
                placeholders.Add(new Placeholder()
                {
                    Key = CreateKey(nameof(ticketCinema.INN)),
                    Value = ticketCinema.INN
                });
            }

            if (!string.IsNullOrEmpty(ticketCinema.Movie))
            {
                placeholders.Add(new Placeholder()
                {
                    Key = CreateKey(nameof(ticketCinema.Movie)),
                    Value = ticketCinema.Movie
                });
            }

            if (!string.IsNullOrEmpty(ticketCinema.Format))
            {
                placeholders.Add(new Placeholder()
                {
                    Key = CreateKey(nameof(ticketCinema.Format)),
                    Value = ticketCinema.Format
                });
            }

            var ticket = new Ticket()
            {
                TemplateName = ticketCinema.TemplateName,
                Placeholders = placeholders.ToArray()
            };

            return _printTicketOperation.Execute(ticket);
        }
    }
}
