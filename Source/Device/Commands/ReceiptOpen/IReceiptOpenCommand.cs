namespace TK302FBPrinter.Device.Commands.ReceiptOpen
{
    public interface IReceiptOpenCommand : IDeviceCommand
    {
        bool Execute(bool isReturn, int taxType, bool print);
    }
}
