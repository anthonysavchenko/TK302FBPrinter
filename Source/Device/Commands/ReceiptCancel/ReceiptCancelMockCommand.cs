using Custom.Fiscal.RUSProtocolAPI;

namespace TK302FBPrinter.Device.Commands.ReceiptCancel
{
    public class ReceiptCancelMockCommand : DeviceCommand, IReceiptCancelCommand
    {
        public ReceiptCancelMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
