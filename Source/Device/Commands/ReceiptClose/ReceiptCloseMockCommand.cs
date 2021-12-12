namespace TK302FBPrinter.Device.Commands.ReceiptClose
{
    public class ReceiptCloseMockCommand : DeviceCommand, IReceiptCloseCommand
    {
        public ReceiptCloseMockCommand() : base(null, null) {}

        public bool Execute(int total)
        {
            return true;
        }
    }
}
