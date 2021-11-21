using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptClose
{
    public class ReceiptCloseMockCommand : DeviceCommand, IReceiptCloseCommand
    {
        public ReceiptCloseMockCommand() : base(null, null) {}

        public bool Execute(ReceiptDto receipt)
        {
            return true;
        }
    }
}
