namespace TK302FBPrinter.Device.Commands.Disconnect
{
    public class DisconnectMockCommand : DeviceCommand, IDisconnectCommand
    {
        public DisconnectMockCommand() : base(null, null)
        {
        }

        public bool Execute()
        {
            return true;
        }
    }
}
