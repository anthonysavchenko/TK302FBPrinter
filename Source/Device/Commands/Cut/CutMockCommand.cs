namespace TK302FBPrinter.Device.Commands.Cut
{
    public class CutMockCommand : DeviceCommand, ICutCommand
    {
        public CutMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}