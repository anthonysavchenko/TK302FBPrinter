using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptOpen
{
    public interface IReceiptOpenCommand : IDeviceCommand
    {
        bool Execute(ReceiptDto receipt, bool isReturnReceipt = false);
    }
}
