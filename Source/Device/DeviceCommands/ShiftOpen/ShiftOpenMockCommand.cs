namespace TK302FBPrinter.Device.DeviceCommands.ShiftOpen
{
    public class ShiftOpenMockCommand : DeviceCommand, IShiftOpenCommand
    {
        public ShiftOpenMockCommand() : base(null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
