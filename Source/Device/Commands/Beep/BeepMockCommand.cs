namespace TK302FBPrinter.Device.Commands.Beep
{
    public class BeepMockCommand : DeviceCommand, IBeepCommand
    {
        public BeepMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
