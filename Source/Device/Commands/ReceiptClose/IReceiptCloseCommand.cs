namespace TK302FBPrinter.Device.Commands.ReceiptClose
{
    public interface IReceiptCloseCommand : IDeviceCommand
    {
        bool Execute(int total);
    }
}
