using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptClose
{
    public interface IReceiptCloseCommand : IDeviceCommand
    {
        bool Execute(ReceiptDto receipt);
    }
}
