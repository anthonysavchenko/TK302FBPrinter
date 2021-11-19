using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Operations.PrintReceipt
{
    public interface IPrintReceiptOperation : IOperation
    {
        bool Execute(ReceiptDto receipt);
    }
}
