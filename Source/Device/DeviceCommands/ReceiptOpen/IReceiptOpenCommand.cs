using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptOpen
{
    public interface IReceiptOpenCommand : IDeviceCommand
    {
        bool Execute(ReceiptDto receipt, bool isReturnReceipt = false);
    }
}
