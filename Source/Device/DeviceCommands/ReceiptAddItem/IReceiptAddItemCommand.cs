using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptAddItem
{
    public interface IReceiptAddItemCommand : IDeviceCommand
    {
        bool Execute(ReceiptItemDto item, bool isReceiptReturn = false);
    }
}
