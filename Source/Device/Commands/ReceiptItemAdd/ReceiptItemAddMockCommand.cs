namespace TK302FBPrinter.Device.Commands.ReceiptItemAdd
{
    public class ReceiptItemAddMockCommand : DeviceCommand, IReceiptItemAddCommand
    {
        public ReceiptItemAddMockCommand() : base(null, null) {}

        public bool Execute(string description, long quantity, long price, int vatType, bool isReturn)
        {
            return true;
        }
    }
}
