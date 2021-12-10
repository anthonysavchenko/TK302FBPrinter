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

        public bool Execute(TicketCinema ticketCinema)
        {
            return _printTicketOperation.Execute(ticketCinema.Ticket);
        }
    }
}
