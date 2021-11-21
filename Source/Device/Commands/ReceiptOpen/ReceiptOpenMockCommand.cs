using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptOpen
{
    public class ReceiptOpenMockCommand : DeviceCommand, IReceiptOpenCommand
    {
        public ReceiptOpenMockCommand() : base(null, null) {}

        public bool Execute(ReceiptDto receipt, bool isReceiptReturn = false)
        {
            return true;
        }
    }
}
