namespace TK302FBPrinter.Device.Commands.Connect
{
    public class ConnectMockCommand : DeviceCommand, IConnectCommand
    {
        public ConnectMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
