using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptClose
{
    public class ReceiptCloseMockCommand : DeviceCommand, IReceiptCloseCommand
    {
        public ReceiptCloseMockCommand() : base(null) {}

        public bool Execute(ReceiptDto receipt)
        {
            return true;
        }
    }
}
