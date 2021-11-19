namespace TK302FBPrinter.Device.DeviceCommands.Beep
{
    public class BeepMockCommand : DeviceCommand, IBeepCommand
    {
        public BeepMockCommand() : base(null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
