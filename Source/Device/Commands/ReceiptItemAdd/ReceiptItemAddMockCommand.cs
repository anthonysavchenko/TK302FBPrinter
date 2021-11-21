using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptItemAdd
{
    public class ReceiptItemAddMockCommand : DeviceCommand, IReceiptItemAddCommand
    {
        public ReceiptItemAddMockCommand() : base(null, null) {}

        public bool Execute(ReceiptItemDto item, bool isReceiptReturn = false)
        {
            return true;
        }
    }
}
