namespace TK302FBPrinter.Business.Operations.PrintTicket
{
    public interface IPrintTicketOperation : IOperation
    {
        bool Execute(string text);
    }
}
