namespace TK302FBPrinter.Device.Commands.ReceiptItemAdd
{
    public interface IReceiptItemAddCommand : IDeviceCommand
    {
        bool Execute(string description, long quantity, long price, int vatType, bool isReturn);
    }
}
