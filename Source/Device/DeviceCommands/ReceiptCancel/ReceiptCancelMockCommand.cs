using Custom.Fiscal.RUSProtocolAPI;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptCancel
{
    public class ReceiptCancelMockCommand : DeviceCommand, IReceiptCancelCommand
    {
        public ReceiptCancelMockCommand() : base(null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
