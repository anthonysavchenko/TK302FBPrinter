namespace TK302FBPrinter.Device.Commands.ReceiptOpen
{
    public class ReceiptOpenMockCommand : DeviceCommand, IReceiptOpenCommand
    {
        public ReceiptOpenMockCommand() : base(null, null) {}

        public bool Execute(bool isReturn, int taxType)
        {
            return true;
        }
    }
}
