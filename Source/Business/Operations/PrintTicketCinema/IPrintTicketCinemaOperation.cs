using TK302FBPrinter.Business.Models;

namespace TK302FBPrinter.Business.Operations.PrintTicketCinema
{
    public interface IPrintTicketCinemaOperation : IOperation
    {
        bool Execute(TicketCinema ticket);
    }
}
