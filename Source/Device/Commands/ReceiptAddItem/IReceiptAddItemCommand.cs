using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptAddItem
{
    public interface IReceiptAddItemCommand : IDeviceCommand
    {
        bool Execute(ReceiptItemDto item, bool isReceiptReturn = false);
    }
}
