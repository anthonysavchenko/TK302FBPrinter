using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Business.Operations.PrintReceipt
{
    public interface IPrintReceiptOperation : IOperation
    {
        bool Execute(ReceiptDto receipt);
    }
}
