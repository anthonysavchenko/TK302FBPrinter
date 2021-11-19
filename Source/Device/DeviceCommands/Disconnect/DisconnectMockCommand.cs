namespace TK302FBPrinter.Device.DeviceCommands.Disconnect
{
    public class DisconnectMockCommand : DeviceCommand, IDisconnectCommand
    {
        public DisconnectMockCommand() : base(null)
        {
        }

        public bool Execute()
        {
            return true;
        }
    }
}
