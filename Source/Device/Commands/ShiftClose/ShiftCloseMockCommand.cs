namespace TK302FBPrinter.Device.Commands.ShiftClose
{
    public class ShiftCloseMockCommand : DeviceCommand, IShiftCloseCommand
    {
        public ShiftCloseMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
