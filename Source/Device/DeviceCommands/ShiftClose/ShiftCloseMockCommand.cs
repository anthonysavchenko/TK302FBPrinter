namespace TK302FBPrinter.Device.DeviceCommands.ShiftClose
{
    public class ShiftCloseMockCommand : DeviceCommand, IShiftCloseCommand
    {
        public ShiftCloseMockCommand() : base(null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
