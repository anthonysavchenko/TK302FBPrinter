using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Operations.PrintReceiptReturn
{
    public interface IPrintReceiptReturnOperation : IOperation
    {
        bool Execute(ReceiptDto receipt);        
    }
}