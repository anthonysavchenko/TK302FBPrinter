namespace TK302FBPrinter.Device.DeviceCommands.Connect
{
    public class ConnectMockCommand : DeviceCommand, IConnectCommand
    {
        public ConnectMockCommand() : base(null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
