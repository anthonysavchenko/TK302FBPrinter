namespace TK302FBPrinter.Device.Commands.CancelLastItem
{
    public class CancelLastItemMockCommand : DeviceCommand, ICancelLastItemCommand
    {
        public CancelLastItemMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
