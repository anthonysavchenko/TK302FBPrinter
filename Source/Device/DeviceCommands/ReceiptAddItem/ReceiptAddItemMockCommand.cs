using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptAddItem
{
    public class ReceiptAddItemMockCommand : DeviceCommand, IReceiptAddItemCommand
    {
        public ReceiptAddItemMockCommand() : base(null)
        {
        }

        public bool Execute(ReceiptItemDto item, bool isReceiptReturn = false)
        {
            return true;
        }
    }
}
