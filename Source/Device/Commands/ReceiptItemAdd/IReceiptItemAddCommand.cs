using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptItemAdd
{
    public interface IReceiptItemAddCommand : IDeviceCommand
    {
        bool Execute(ReceiptItemDto item, bool isReceiptReturn = false);
    }
}
