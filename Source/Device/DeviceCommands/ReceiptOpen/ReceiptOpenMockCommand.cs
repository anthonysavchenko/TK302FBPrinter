using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptOpen
{
    public class ReceiptOpenMockCommand : DeviceCommand, IReceiptOpenCommand
    {
        public ReceiptOpenMockCommand() : base(null) {}

        public bool Execute(ReceiptDto receipt)
        {
            return true;
        }
    }
}
