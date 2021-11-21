using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptClose
{
    public interface IReceiptCloseCommand : IDeviceCommand
    {
        bool Execute(ReceiptDto receipt);
    }
}
