using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Business.Operations.PrintTicket
{
    public interface IPrintTicketOperation : IOperation
    {
        bool Execute(TicketDto ticket);
    }
}
