using TK302FBPrinter.Business.Models;

namespace TK302FBPrinter.Business.Operations.PrintTicket
{
    public interface IPrintTicketOperation : IOperation
    {
        bool Execute(Ticket ticket, bool cut = false);
    }
}
