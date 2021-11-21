using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.ReceiptAddItem
{
    public class ReceiptAddItemMockCommand : DeviceCommand, IReceiptAddItemCommand
    {
        public ReceiptAddItemMockCommand() : base(null, null) {}

        public bool Execute(ReceiptItemDto item, bool isReceiptReturn = false)
        {
            return true;
        }
    }
}
