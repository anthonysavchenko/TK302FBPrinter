namespace TK302FBPrinter.Device.Commands.ReceiptItemCancel
{
    public class ReceiptItemCancelMockCommand : DeviceCommand, IReceiptItemCancelCommand
    {
        public ReceiptItemCancelMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
