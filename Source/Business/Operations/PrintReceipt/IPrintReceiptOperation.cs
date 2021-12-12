using TK302FBPrinter.Business.Models;

namespace TK302FBPrinter.Business.Operations.PrintReceipt
{
    public interface IPrintReceiptOperation : IOperation
    {
        bool Execute(Receipt receipt);
    }
}
